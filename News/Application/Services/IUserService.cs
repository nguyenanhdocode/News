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

        /// <summary>
        /// Register user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task Register(RegisterModel model);

        /// <summary>
        /// Confirm email
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task ConfirmEmail(ConfirmEmailModel model);

        /// <summary>
        /// Get all user
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<List<ProfileModel>> GetAll(Dictionary<string, string> param);

        /// <summary>
        /// Get profile id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProfileModel> GetProfile(string id);

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task Delete(string userId);

        /// <summary>
        /// Lock user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task LockUser(string userId);

        /// <summary>
        /// Update roles
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        Task UpdateRoles(UpdateRolesModel model);

        /// <summary>
        /// Check if user is in role
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<bool> IsInRole(string userId, string role);
    }
}
