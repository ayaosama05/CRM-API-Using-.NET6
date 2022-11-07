using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Identity;
using System.Xml.Linq;

namespace CustomerRelationshipManagementAPI.Core.Repositories
{
    public class AdministrationRepository : IAdministrationRepository
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AdministrationRepository(RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager
            )
        {
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<RoleModel> AddNewRoleAsync(string name)
        {
            if (await _roleManager.RoleExistsAsync(name))
                return new RoleModel { RoleName = name, IsSucceeded = false, Message = "This role is already existed" };

            if (string.IsNullOrEmpty(name))
                return new RoleModel { RoleName = name, IsSucceeded = false, Message = "Role must have a name" };

            var result = await _roleManager.CreateAsync(new IdentityRole(name));

            if (!result.Succeeded)
            {
                string errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description} ,";
                    return new RoleModel { RoleName = name, IsSucceeded = false, Message = $"{errors}" };
                }
            }

            return new RoleModel { RoleName = name, IsSucceeded = true, Message = $"Role {name} is added successfully" };
        }

        public async Task<UserRoleModel> AddUserToRoleAsync(string userName , string roleName)
        {
            if (string.IsNullOrEmpty(roleName) || string.IsNullOrEmpty(userName))
                return new UserRoleModel { UserName = userName, RoleName = roleName,
                    IsSucceeded = false, Message = "You must provide a Role & User Name" };

            var role = await _roleManager.FindByNameAsync(roleName);
            var user = await _userManager.FindByNameAsync(userName);

            if (role is null)
                return new UserRoleModel { UserName = userName, RoleName = roleName, IsSucceeded = false, Message = "Role not found" };

            if (user is null)
                return new UserRoleModel { UserName = userName, RoleName = roleName, IsSucceeded = false, Message = "User not found" };

            var result = await _userManager.AddToRoleAsync(user,roleName);

            if (!result.Succeeded)
            {
                string errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description} ,";
                    return new UserRoleModel { UserName = userName, RoleName = roleName, IsSucceeded = false, Message = $"{errors}" };
                }
            }

            return new UserRoleModel { UserName = userName, RoleName = roleName, IsSucceeded = true, Message = $"{userName} is added to {roleName} role successfully" };
        }
    }
}
