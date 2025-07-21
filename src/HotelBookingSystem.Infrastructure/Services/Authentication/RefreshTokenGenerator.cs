using System.Security.Cryptography;
using HotelBookingSystem.Application.Common.Interfaces.Authentication;
using HotelBookingSystem.Domain.Entities;
using HotelBookingSystem.Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace HotelBookingSystem.Infrastructure.Services.Authentication;

public class RefreshTokenGenerator : IRefreshTokenGenerator
{
    private readonly JwtOptions _jwtOptions;

    public RefreshTokenGenerator(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public RefreshToken GenerateToken(Guid userId)
    {
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)),
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(Convert.ToInt32(_jwtOptions.RefreshTokenLifeTimeDays)),
            UserId = userId,
            IsRevoked = false
        };

        return refreshToken;
    }
}