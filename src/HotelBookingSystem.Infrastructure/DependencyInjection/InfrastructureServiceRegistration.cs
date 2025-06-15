using HotelBookingSystem.Application.Common.Interfaces.Authentication;
using HotelBookingSystem.Application.Common.Interfaces.Users;
using HotelBookingSystem.Infrastructure.Authentication;
using HotelBookingSystem.Infrastructure.Security;
using HotelBookingSystem.Infrastructure.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotelBookingSystem.Infrastructure.DependencyInjection;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
        services.AddScoped<IRefreshTokenCleaner, RefreshTokenCleaner>();
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}