using FluentResults;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using HotelBookingSystem.Application.Common.Interfaces.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Application.Features.Moderation.Users.Commands.AssignRoleToUser;

public class AssignRoleToUserCommandHandler : IRequestHandler<AssignRoleToUserCommand, Result>
{
    private readonly ICurrentUserService _currentUser;
    private readonly IHotelBookingDbContext _context;
    private readonly ILogger<AssignRoleToUserCommandHandler> _logger;
    
    public AssignRoleToUserCommandHandler(ICurrentUserService currentUser, IHotelBookingDbContext context, ILogger<AssignRoleToUserCommandHandler> logger)
    {
        _currentUser = currentUser;
        _context = context;
        _logger = logger;
    }

    public async Task<Result> Handle(AssignRoleToUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null)
        {
            _logger.LogWarning("User with ID {UserId} not found", request.UserId);
            return Result.Fail("User not found");
        }
        
        var role = await _context.Roles
            .FirstOrDefaultAsync(r => r.Id == request.RoleId, cancellationToken);
        
        if (role == null)
        {
            _logger.LogWarning("Role with ID {RoleId} not found", request.RoleId);
            return Result.Fail("Role not found");
        }
        
        if (_currentUser.GetUserId() == request.UserId && !_currentUser.IsAdmin())
        {
            _logger.LogWarning("User {UserId} attempted to assign role {RoleName} to themselves", request.UserId, role.Name);
            return Result.Fail("You cannot assign a role to yourself");
        }
        
        if (_currentUser.IsModerator() && role.Name != "Manager")
        {
            _logger.LogWarning("Moderator attempted to assign role {RoleName} to user {UserId}", role.Name, request.UserId);
            return Result.Fail("Moderators can only assign the 'Manager' role");
        }

        if (user.Roles.Any(r => r.Id == role.Id))
        {
            _logger.LogWarning("User with ID {UserId} already has role {RoleName}", request.UserId, role.Name);
            return Result.Fail("User already has this role");
        }
        
        user.Roles.Add(role);
        await _context.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Role {RoleName} assigned to user {UserId} by {CurrentUserId}",
            role.Name, request.UserId, _currentUser.GetUserId());
        
        return Result.Ok();
    }
}