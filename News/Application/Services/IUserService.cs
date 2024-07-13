using Application.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ClaimsPrincipal> Login(LoginModel model);

        /// <summary>s
        /// Get own profile
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<OwnProfileResponseModel> GetProfile();

        /// <summary>
        /// Create forgot password request
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task CreateForgotPasswordRequest(ForgotPasswordModel model);

        /// <summary>
        /// Reset password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task ResetPassword(ResetPasswordModel model);

        /// <summary>
        /// Get user access failed count
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<int> GetAccessFailedCount(string email);
    }
}
