using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotelBookingSystem.Persistence.DependencyInjection;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration) 
    {
        services.AddDbContext<HotelBookingDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                sql => sql.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));
        
        services.AddScoped<IHotelBookingDbContext>(provider => provider.GetRequiredService<HotelBookingDbContext>());
        
        return services;
    }
}