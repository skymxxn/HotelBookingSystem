using System.Security.Cryptography;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace HotelBookingSystem.Infrastructure.Authentication;

public class RefreshTokenGenerator : IRefreshTokenGenerator
{
    private readonly IConfiguration _configuration;

    public RefreshTokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public RefreshToken GenerateToken(Guid userId)
    {
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)),
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(Convert.ToInt32(_configuration["JwtSettings:RefreshTokenLifeTimeDays"])),
            UserId = userId,
            IsRevoked = false
        };

        return refreshToken;
    }
}