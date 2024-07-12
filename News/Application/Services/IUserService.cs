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
        Task<ClaimsPrincipal> Login(LoginModel model);

        /// <summary>s
        /// Get own profile
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<OwnProfileResponseModel> GetProfile();
    }
}
