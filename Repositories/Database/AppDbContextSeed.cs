using ExamApp.Repositories.Enums;
using ExamApp.Repositories.Users;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace ExamApp.Repositories.Database
{
    public static class AppDbContextSeed
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            // Eğer en az bir admin yoksa ekle
            if (!context.Users.Any(u => u.Role == UserRole.Admin))
            {
                var adminUser = new User
                {
                    FullName = "Admin",
                    Email = "admin@admin.com",
                    Password = Hash("Admin123"),
                    Role = UserRole.Admin,
                    IsDeleted = false
                };

                context.Users.Add(adminUser);
                await context.SaveChangesAsync();
            }
        }

        public static string Hash(string password)
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            byte[] hash = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, 100000, 32);

            return Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
        }
    }


}
