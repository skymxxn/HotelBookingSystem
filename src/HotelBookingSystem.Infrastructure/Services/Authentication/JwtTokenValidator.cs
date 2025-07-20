using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HotelBookingSystem.Application.Common.Interfaces.Authentication;
using HotelBookingSystem.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HotelBookingSystem.Infrastructure.Services.Authentication;

public class JwtTokenValidator  : IJwtTokenValidator
{
    private readonly JwtOptions  _jwtOptions;
    private readonly ILogger<JwtTokenValidator> _logger;

    public JwtTokenValidator(ILogger<JwtTokenValidator> logger, IOptions<JwtOptions> jwtOptions)
    {
        _logger = logger;
        _jwtOptions = jwtOptions.Value;
    }

    private ClaimsPrincipal? ValidateTokenInterval(string token, JwtTokenTypeOptions options)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(options.Key);

        try
        {
            var parameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = options.Issuer,
                ValidateAudience = true,
                ValidAudience = options.Audience,
                ClockSkew = TimeSpan.Zero
            };
            
            var principal = tokenHandler.ValidateToken(token, parameters, out _);
            return principal;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return null;
        }
    }
    
    public Guid? ValidateEmailVerificationToken(string token)
    {
        var principal = ValidateTokenInterval(token, _jwtOptions.EmailVerification);
        var userIdClaim = principal?.FindFirst(ClaimTypes.NameIdentifier);
        return userIdClaim is not null ? Guid.Parse(userIdClaim.Value) : null;
    }
}