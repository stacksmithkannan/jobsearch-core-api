using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using JobFinder.API.Data;
using JobFinder.API.Application.Models;
using JobFinder.API.Domain.Entities;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;


namespace JobFinder.API.StartUp
{
    public class SeedAdminUser
    {
        public static async Task EnsureAdminCreatedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>()
                                .CreateLogger("AdminSeeder");
            var config = scope.ServiceProvider.GetRequiredService<IOptions<AdminUserOptions>>();

            var adminOptions = config.Value;

            if (await context.Users.AnyAsync(u => u.Role == "Admin"))
            {
                logger.LogInformation("Admin user already exists.");
                return;
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(adminOptions.Password);

            var adminUser = new User
            {
                UserName = adminOptions.UserName,
                Email = adminOptions.Email,
                Password = hashedPassword,
                Role = "Admin"
            };

            context.Users.Add(adminUser);
            await context.SaveChangesAsync();

            logger.LogInformation("Admin user seeded successfully.");
        }
    }
}
