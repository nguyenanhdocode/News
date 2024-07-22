using Application.Common.Email;
using Application.Exceptions;
using Application.Models.TwoFactorAuth;
using AutoMapper;
using Core.Entities;
using DataAccess.UnifOfWork;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MimeKit.Encodings;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Impl
{
    public class TwoFactorAuthService : ITwoFactorAuthService
    {
        private IUnitOfWork _uow;
        private IMapper _mapper;
        private IEmailService _emailService;
        private ITemplateService _templateService;
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;

        public TwoFactorAuthService(IUnitOfWork unitOfWork
            , IMapper mapper
            , IEmailService emailService
            , ITemplateService templateService
            , UserManager<AppUser> userManager
            , SignInManager<AppUser> signInManager)
        {
            _uow = unitOfWork;
            _mapper = mapper;
            _emailService = emailService;
            _templateService = templateService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task Create(CreateTwoFactorAuthModel model)
        {
            var tfa = _mapper.Map<CreateTwoFactorAuthModel, TwoFactorAuth>(model);
            await _uow.TfaRepository.Add(tfa);
            await _uow.SaveChangesAsync();
        }

        public async Task IncreaseFailedCount(TwoFactorAuth model)
        {
            model.FailedCount += 1;
            _uow.TfaRepository.Update(model);
            await _uow.SaveChangesAsync();
        }

        public async Task<ClaimsPrincipal> Login(TwoFactorAuthModel model)
        {
            var tfa = await _uow.TfaRepository.Get(model.Token, model.UserId);

            if (tfa == null)
                throw new NotFoundException("Invalid token");

            if (_uow.TfaRepository.IsExpires(tfa))
                throw new NotFoundException("Invalid token");

            if (tfa.FailedCount >= 5)
                throw new ForbiddenException();

            if (!_uow.TfaRepository.Verify(tfa, model.Code))
            {
                _uow.TfaRepository.IncreaseFailedCount(tfa);
                await _uow.SaveChangesAsync();
                throw new UnauthorizeException("Authenticate failed");
            }

            _uow.TfaRepository.Delete(tfa);
            await _uow.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(model.UserId);

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>() {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            return principal;
        }

        public async Task SendCode(SendCodeModel model)
        {
            string template = await _templateService.GetTemplateAsync("two_factor_auth.html");
            string body = _templateService.ReplaceInTemplate(template, new Dictionary<string, string>
            {
                {"{firstName}", model.FirstName },
                {"{lastName}", model.LastName },
                {"{code}", model.Code}
            });

            string subject = "N4News - Xác thực 2 bước";
            await _emailService.SendEmailAsync(new EmailMessage(model.Email, body, subject));
        }

        public async Task Update(TwoFactorAuth model)
        {
            _uow.TfaRepository.Update(model);
            await _uow.SaveChangesAsync();
        }

        public async Task<bool> Validate(string token, string userId)
        {
            var tfa = await _uow.TfaRepository.Get(p => p.UserId.Equals(userId)
                && p.Token.Equals(token)
                && !p.IsAuthenticated
                && DateTime.Compare(p.Expires, DateTime.UtcNow) > 0);

            return tfa != null;
        }
    }
}
