using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using HotelBookingSystem.Application.Common.Interfaces.Authentication;
using HotelBookingSystem.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace HotelBookingSystem.Infrastructure.Services.Authentication;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtOptions _jwtOptions;

    public JwtTokenGenerator(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public string GenerateToken(Guid userId, string email, IEnumerable<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        
        claims.AddRange(roles.Select(role => new Claim("role", role)));

        return JwtHelper.GenerateToken(
            claims,
            _jwtOptions.AccessToken.Key,
            _jwtOptions.AccessToken.Issuer,
            _jwtOptions.AccessToken.Audience,
            _jwtOptions.AccessToken.LifetimeMinutes
            );
    }
    
    public string GenerateEmailVerificationToken(Guid userId)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        return JwtHelper.GenerateToken(
            claims,
            _jwtOptions.EmailVerification.Key,
            _jwtOptions.EmailVerification.Issuer,
            _jwtOptions.EmailVerification.Audience,
            _jwtOptions.EmailVerification.LifetimeMinutes
        );
    }
}