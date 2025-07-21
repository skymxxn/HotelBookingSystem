using HotelBookingSystem.Persistence;
using HotelBookingSystem.Persistence.Seeders;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.API.Extensions;

public static class HostExtensions
{
    public static async Task MigrateAndSeedAsync(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<HotelBookingDbContext>();

        try
        {
            await dbContext.Database.MigrateAsync();
            await DbSeeder.SeedRolesAsync(dbContext);
        }
        catch (Exception ex)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<HotelBookingDbContext>>();
            logger.LogError(ex, "An error occurred while migrating or seeding the database.");
            throw;
        }
    }
}