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
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<HotelBookingDbContext>>();
        var services = scope.ServiceProvider;
        
        try
        {
            await dbContext.Database.MigrateAsync();
            logger.LogInformation("Database migrated successfully.");
            await DbSeeder.SeedRolesAsync(dbContext);
            await DbSeeder.SeedAdminUserAsync(services);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating or seeding the database.");
            throw;
        }
    }
}