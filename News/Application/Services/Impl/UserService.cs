using Application.Exceptions;
using Application.Models.User;
using AutoMapper;
using Core.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Impl
{
    public class UserService : IUserService
    {
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private IMapper _mapper;
        private IClaimService _claimService;

        public UserService(UserManager<AppUser> userManager
            , SignInManager<AppUser> signInManager
            , IMapper mapper
            , IClaimService claimService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _claimService = claimService;
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
                throw new UnauthorizeException("Username or password is incorrect");

            var claims = new List<Claim>() {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            return principal;
        }
    }
}
