using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace HotelBookingSystem.Infrastructure.Services.Authentication;

public static class JwtAuthenticationSetup
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtSection = configuration.GetSection("JwtOptions:AccessToken");
        var key = Encoding.UTF8.GetBytes(jwtSection["Key"] ?? string.Empty);
        
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    RoleClaimType = ClaimTypes.Role,
        
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
        
                    ValidateIssuer = true,
                    ValidIssuer = jwtSection["Issuer"],
        
                    ValidateAudience = true,
                    ValidAudience = jwtSection["Audience"],
        
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero 
                };
            });
        
        return services;
    }
}