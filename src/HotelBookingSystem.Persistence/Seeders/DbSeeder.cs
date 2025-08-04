using HotelBookingSystem.Application.Common.Interfaces.Authentication;
using HotelBookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Persistence.Seeders;

public static class DbSeeder
{
    public static async Task SeedRolesAsync(HotelBookingDbContext context)
    {
        if (!await context.Roles.AnyAsync())
        {
            var roles = new[]
            {
                new Role { Id = Guid.NewGuid(), Name = "User" },
                new Role { Id = Guid.NewGuid(), Name = "Manager" },
                new Role { Id = Guid.NewGuid(), Name = "Moderator" },
                new Role { Id = Guid.NewGuid(), Name = "Admin" }
            };
            
            await context.Roles.AddRangeAsync(roles);
            await context.SaveChangesAsync();
        }
    }

    public static async Task SeedAdminUserAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HotelBookingDbContext>();
        var  passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("DbSeeder");
        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        var adminEmail = config["DEFAULT_ADMIN_EMAIL"] ?? "admin@hotel.com";
        var adminPassword = config["DEFAULT_ADMIN_PASSWORD"] ?? "admin";

        if (await context.Users.AnyAsync(u => u.Email == adminEmail))
        {
            logger.LogWarning("Admin user already exists");
            return;
        }
        
        var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
        if (adminRole == null)
        {
            logger.LogWarning("Admin role not found");
            return;
        }

        var adminUser = new User()
        {
            FirstName = "Admin",
            LastName = "User",
            Email = adminEmail,
            IsEmailConfirmed = true,
            PasswordHash = passwordHasher.Hash(adminPassword),
            PhoneNumber = "0000000000",
            CreatedAt = DateTime.UtcNow,
            Roles = new List<Role> { adminRole }
        };
        
        context.Users.Add(adminUser);
        await context.SaveChangesAsync();
        
        logger.LogWarning("Admin user added successfully");
    }
}