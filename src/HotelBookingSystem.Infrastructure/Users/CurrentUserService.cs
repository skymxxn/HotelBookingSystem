using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using HotelBookingSystem.Application.Common.Interfaces.Users;
using Microsoft.AspNetCore.Http;

namespace HotelBookingSystem.Infrastructure.Users;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User?
            .FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var parsedUserId)) 
        {
            throw new InvalidOperationException("User ID is not available or invalid.");
        }
        
        return parsedUserId;
    }

    public List<string> GetRoles()
    {
        var roles = _httpContextAccessor.HttpContext?.User?
            .FindAll(ClaimTypes.Role)
            .Select(role => role.Value)
            .ToList();

        if (roles == null || !roles.Any())
        {
            throw new InvalidOperationException("User roles are not available.");
        }

        return roles;
    }
}