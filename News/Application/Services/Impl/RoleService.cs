using Core.Entities;
using LinqKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Impl
{
    public class RoleService : IRoleService
    {
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<AppUser> _userManager;

        public RoleService(RoleManager<IdentityRole> roleManager
            , UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<List<IdentityRole>> GetAll(List<string> excludedRoleIds)
        {
            var predicate = PredicateBuilder.New<IdentityRole>(true);

            if (excludedRoleIds != null)
            {
                predicate.And(p => !excludedRoleIds.Any(x => x.Equals(p.Name)));
            }

            return (await _roleManager.Roles.ToListAsync())
                .Where(predicate)
                .ToList();
        }

        public async Task<List<string>> GetRolesOfUser(string userId)
        {
            var roles = new List<IdentityRole>();
            var user = await _userManager.FindByIdAsync(userId);
            return (List<string>)await _userManager.GetRolesAsync(user);
        }
    }
}
