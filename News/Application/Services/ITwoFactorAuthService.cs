using Application.Models.TwoFactorAuth;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface ITwoFactorAuthService
    {
        Task Create(CreateTwoFactorAuthModel model);
        Task SendCode(SendCodeModel model);
        Task<bool> Validate(string token, string userId);
        Task<ClaimsPrincipal> Login(TwoFactorAuthModel model);
        Task IncreaseFailedCount(TwoFactorAuth model);
        Task Update(TwoFactorAuth model);
    }
}
