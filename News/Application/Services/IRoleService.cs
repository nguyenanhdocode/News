using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IRoleService
    {
        Task<List<IdentityRole>> GetAll(List<string> excludedRoles);
        Task<List<string>> GetRolesOfUser(string userId);
    }
}
