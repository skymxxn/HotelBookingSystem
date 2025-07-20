using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using HotelBookingSystem.Application.Common.Interfaces.Authentication;
using HotelBookingSystem.Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HotelBookingSystem.Infrastructure.Services.Authentication;

public class JwtTokenValidator  : IJwtTokenValidator
{
    private readonly JwtOptions _jwtOptions;
    private readonly ILogger<JwtTokenValidator> _logger;

    public JwtTokenValidator(ILogger<JwtTokenValidator> logger, IOptions<JwtOptions> jwtOptions)
    {
        _logger = logger;
        _jwtOptions = jwtOptions.Value;
    }

    public Guid? ValidateEmailVerificationToken(string token)
    {
        try
        {
            var principal = JwtHelper.ValidateToken(
                token,
                _jwtOptions.EmailVerification.Key,
                _jwtOptions.EmailVerification.Issuer,
                _jwtOptions.EmailVerification.Audience,
                out _
            );
            
            var subClaim = principal?.FindFirst(ClaimTypes.NameIdentifier);
            return subClaim is not null ? Guid.Parse(subClaim.Value) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Email token validation failed: {Message}", ex.Message);
            return null;
        }
    }
}