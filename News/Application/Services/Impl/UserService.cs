using Application.Common.Email;
using Application.Exceptions;
using Application.Models.User;
using AutoMapper;
using Core.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Application.Services.Impl
{
    public class UserService : IUserService
    {
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private IMapper _mapper;
        private IClaimService _claimService;
        private IEmailService _emailService;
        private ITemplateService _templateService;
        private IHttpContextAccessor _contextAccessor;

        public UserService(UserManager<AppUser> userManager
            , SignInManager<AppUser> signInManager
            , IMapper mapper
            , IClaimService claimService
            , ITemplateService templateService
            , IHttpContextAccessor httpContextAccessor
            , IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _claimService = claimService;
            _templateService = templateService;
            _contextAccessor = httpContextAccessor;
            _emailService = emailService;
        }

        public async Task ConfirmEmail(ConfirmEmailModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null || user.EmailConfirmed)
                throw new UnprocessableEntityException("Your verification link is incorrect");

            var result = await _userManager.ConfirmEmailAsync(user, model.Token.Replace(" ", "+"));
            if (!result.Succeeded)
                throw new UnprocessableEntityException("Your verification link is incorrect");
        }

        public async Task CreateForgotPasswordRequest(ForgotPasswordModel model)
        {
            AppUser user = await _userManager.FindByEmailAsync(model.Email);
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            string template = await _templateService.GetTemplateAsync("reset_password.html");
            string url = string.Format("{0}://{1}{2}"
                , _contextAccessor.HttpContext.Request.Scheme
                , _contextAccessor.HttpContext.Request.Host
                , _contextAccessor.HttpContext.Request.PathBase);
            string link = string.Format("{0}/Users/ResetPassword?Token={1}&UserId={2}"
                , url, token, user.Id);

            string body = _templateService.ReplaceInTemplate(template
                , new Dictionary<string, string>
            {
                    { "{firstName}", user.FirstName },
                    { "{lastName}", user.LastName },
                    { "{resetLink}", link }
            });

            await _emailService.SendEmailAsync(new EmailMessage(model.Email, body, "Reset password"));
        }

        public async Task<int> GetAccessFailedCount(string email)
        {
            AppUser user = await _userManager.FindByEmailAsync(email);
            return user != null ? user.AccessFailedCount : 0;
        }

        public async Task<OwnProfileResponseModel> GetProfile()
        {
            string userId = _claimService.GetUserId();
            AppUser? user = await _userManager.Users.SingleOrDefaultAsync(p => p.Id.Equals(userId));

            return _mapper.Map<OwnProfileResponseModel>(user);
        }

        public async Task<ClaimsPrincipal> Login(LoginModel model)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName.Equals(model.Email));

            if (user == null)
                throw new UnauthorizeException("Username or password is incorrect");

            var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

            if (!signInResult.Succeeded)
            {
                await _userManager.AccessFailedAsync(user);
                throw new UnauthorizeException("Username or password is incorrect");
            }

            await _userManager.ResetAccessFailedCountAsync(user);

            var claims = new List<Claim>() {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            return principal;
        }

        public async Task Register(RegisterModel model)
        {

            var user = _mapper.Map<AppUser>(model);
            user.UserName = model.Email;
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                throw new UnprocessableEntityException(result.Errors.FirstOrDefault()?.Description);

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            string content = await _templateService.GetTemplateAsync("confirm_email.html");
            string url = string.Format("{0}://{1}{2}"
                , _contextAccessor.HttpContext.Request.Scheme
                , _contextAccessor.HttpContext.Request.Host
                , _contextAccessor.HttpContext.Request.PathBase);
            string link = string.Format("{0}/Users/ConfirmEmail?Token={1}&UserId={2}"
                , url, token, user.Id);
            string body = _templateService.ReplaceInTemplate(content, new Dictionary<string, string>()
                        {
                            { "{firstName}", model.FirstName },
                            { "{lastName}", model.LastName},
                            { "{activateLink}", link }
                        });

            await _emailService.SendEmailAsync(new EmailMessage(model.Email, body, "Activate account"));
        }

        public async Task ResetPassword(ResetPasswordModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
                throw new UnprocessableEntityException("Invalid token");

            var result = await _userManager.ResetPasswordAsync(user
                , model.Token.Replace(" ", "+"), model.NewPassword);
            if (!result.Succeeded)
                throw new UnprocessableEntityException(result.Errors.FirstOrDefault()?.Description);
        }
    }
}
