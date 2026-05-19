using ProjectManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ProjectManagement.Infrastructure.Seeder
{
    public static class UserSeeder
    {
        public static async Task SeedAsync(UserManager<User> _userManager)
        {
            var usersCount = await _userManager.Users.CountAsync();
            if (usersCount <= 0)
            {
                var defaultUser = new User()
                {
                    UserName = "Conquer",
                    NormalizedUserName = "CONQUER",
                    Email = "mohamedaboelez334@.com",
                    NormalizedEmail = "MOHAMEDABOELEZ334@GMAIL.COM",
                    Name = "Mohamed Ibrahim Hassan",
                    NameLocalization = "محمد ابراهيم حسن",
                    CreationDate = DateTime.Now,
                    CreatorName = "System",
                    PhoneNumber = "+201068706845",
                    Address = "Egypt",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                };
                await _userManager.CreateAsync(defaultUser, "Mohamed.123");
                await _userManager.AddToRoleAsync(defaultUser, "Admin");
            }
        }
    }
}
