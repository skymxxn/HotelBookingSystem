using System.Security.Claims;
using HotelBookingSystem.Application.Common.Interfaces.Users;
using Microsoft.AspNetCore.Http;

namespace HotelBookingSystem.Infrastructure.Services.Users;

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
    
    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public List<string> GetRoles()
    {
        var roles = User?
            .FindAll(ClaimTypes.Role)
            .Select(role => role.Value)
            .ToList();

        if (roles == null || !roles.Any())
        {
            throw new InvalidOperationException("User roles are not available.");
        }

        return roles;
    }

    public string GetUserEmail()
    {
        var userEmail = User?.FindFirst(ClaimTypes.Email)?.Value;
        
        if (string.IsNullOrEmpty(userEmail))
        {
            throw new InvalidOperationException("User email is not available.");
        }
        
        return userEmail;
    }
    
    public bool IsAdmin() => User?.IsInRole("Admin") ?? false;
    
    public bool IsModerator() => User?.IsInRole("Moderator") ?? false;
    
    public bool IsManager() => User?.IsInRole("Manager") ?? false;
    
    public bool IsUser() => User?.IsInRole("User") ?? false;
}