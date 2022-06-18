using Microsoft.AspNetCore.Identity;
using SchoolManagementSystem.Constants;

namespace SchoolManagementSystem.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedRolesAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Principal.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Student.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Teacher.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Parent.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Secretary.ToString()));
        }
    }
}
