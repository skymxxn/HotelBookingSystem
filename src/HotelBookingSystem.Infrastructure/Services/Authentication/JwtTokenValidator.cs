using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HotelBookingSystem.Application.Common.Interfaces.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace HotelBookingSystem.Infrastructure.Services.Authentication;

public class JwtTokenValidator  : IJwtTokenValidator
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<JwtTokenValidator> _logger;

    public JwtTokenValidator(IConfiguration configuration, ILogger<JwtTokenValidator> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public Guid? ValidateEmailVerificationToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["JWTSettings:EmailVerificationTokenKey"]!);

        try
        {
            var parameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _configuration["JwtSettings:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["JwtSettings:Audience"],
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(token, parameters, out _);
            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);

            return userIdClaim is not null ? Guid.Parse(userIdClaim.Value) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return null;
        }
    }
}