using HotelBookingSystem.Application.Common.Interfaces.Access;
using HotelBookingSystem.Application.Common.Interfaces.Authentication;
using HotelBookingSystem.Application.Common.Interfaces.Cache;
using HotelBookingSystem.Application.Common.Interfaces.Email;
using HotelBookingSystem.Application.Common.Interfaces.Users;
using HotelBookingSystem.Infrastructure.Options;
using HotelBookingSystem.Infrastructure.Services.Access;
using HotelBookingSystem.Infrastructure.Services.Authentication;
using HotelBookingSystem.Infrastructure.Services.Cache;
using HotelBookingSystem.Infrastructure.Services.Email;
using HotelBookingSystem.Infrastructure.Services.RateLimiting;
using HotelBookingSystem.Infrastructure.Services.Security;
using HotelBookingSystem.Infrastructure.Services.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotelBookingSystem.Infrastructure.DependencyInjection;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IJwtTokenValidator, JwtTokenValidator>();
        services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
        services.AddScoped<IRefreshTokenCleaner, RefreshTokenCleaner>();
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IAccessService, AccessService>();

        services.AddJwtAuthentication(configuration);
        services.AddRateLimiting();
        
        services.Configure<SmtpOptions>(configuration.GetSection("SmtpOptions"));
        services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));
        services.AddTransient<IEmailService, EmailService>();

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration["Redis:Configuration"];
            options.InstanceName = configuration["Redis:InstanceName"];
        });
        
        services.AddSingleton<IRedisCacheService, RedisCacheService>();

        return services;
    }
}