using ProjectManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ProjectManagement.Infrastructure.Seeder
{
    public static class RoleSeeder
    {
        public static async Task SeedAsync(RoleManager<Role> _roleManager)
        {
            var rolesCount = await _roleManager.Roles.CountAsync();
            if (rolesCount <= 0)
            {

                await _roleManager.CreateAsync(new Role()
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    NameLocalization = "مشرف",
                    CreationDate = DateTime.Now,
                    CreatorName = "System",
                });
                await _roleManager.CreateAsync(new Role()
                {
                    Name = "User",
                    NormalizedName = "USER",
                    NameLocalization = "مستخدم",
                    CreationDate = DateTime.Now,
                    CreatorName = "System",
                });
            }
        }
    }
}
