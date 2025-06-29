using HotelBookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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
}