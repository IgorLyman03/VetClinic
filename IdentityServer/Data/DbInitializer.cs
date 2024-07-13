using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Data;
using System.Runtime.Intrinsics.X86;

namespace IdentityServer.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {

            if (!context.Database.EnsureCreated())
            {
                return;
            }

            await CreateRolesAsync(roleManager);
            await CreateUsersAsync(userManager);

        }

        private static async Task CreateRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            var roles = new[] { "user", "doctor", "admin" };

            foreach (var roleName in roles)
            {
                if (await roleManager.RoleExistsAsync(roleName))
                {
                    continue;
                }
                
                var role = new IdentityRole(roleName);
                var result = await roleManager.CreateAsync(role);
                if (!result.Succeeded)
                {
                    throw new Exception($"Failed to create role {roleName}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }

        private static async Task CreateUsersAsync(UserManager<IdentityUser> userManager)
        {
            var users = new[]
            {
                new { Id = "f82ed7b4-6bd8-4a88-b5ff-62166e6480b8", UserName = "user1", Email = "user1@example.com", Role = "user" },
                new { Id = "18403cdd-cb04-411e-afe5-fe8aa5ed30a3", UserName = "doctor1", Email = "doctor1@example.com", Role = "doctor" },
                new { Id = "8052ba8a-2b27-4d3e-83f5-c1db9f35f055", UserName = "admin1", Email = "admin1@example.com", Role = "admin" }
            };

            foreach (var userInfo in users)
            {
                var user = await userManager.FindByEmailAsync(userInfo.Email);

                if (user == null)
                {
                    user = new IdentityUser { Id = userInfo.Id, UserName = userInfo.UserName, Email = userInfo.Email, EmailConfirmed = true };
                    var result = await userManager.CreateAsync(user, "pass");
                    if (!result.Succeeded)
                    {
                        throw new Exception($"Failed to create user {userInfo.Email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }

                if (!await userManager.IsInRoleAsync(user, userInfo.Role))
                {
                    var result = await userManager.AddToRoleAsync(user, userInfo.Role);
                    if (!result.Succeeded)
                    {
                        throw new Exception($"Failed to add user {userInfo.Email} to role {userInfo.Role}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }
            }
        }

    }
}
