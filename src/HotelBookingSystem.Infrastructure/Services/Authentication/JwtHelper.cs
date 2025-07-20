using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace HotelBookingSystem.Infrastructure.Services.Authentication;

public static class JwtHelper
{
    public static string GenerateToken(
        IEnumerable<Claim> claims,
        string key,
        string issuer,
        string audience,
        int lifetimeMinutes)
    {
        var symmetricKey = new SymmetricSecurityKey(Convert.FromBase64String(key));
        var creds = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(lifetimeMinutes),
            signingCredentials: creds
            );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static ClaimsPrincipal? ValidateToken(
        string token,
        string key,
        string issuer,
        string audience,
        out SecurityToken validatedToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParams = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(key)),
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ClockSkew = TimeSpan.Zero
        };
        
        return tokenHandler.ValidateToken(token, validationParams, out validatedToken);
    }
}