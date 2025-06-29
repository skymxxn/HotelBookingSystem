using FluentResults;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using HotelBookingSystem.Application.Common.Interfaces.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Application.Features.Moderation.Users.Commands.RemoveRoleFromUser;

public class RemoveRoleFromUserCommandHandler : IRequestHandler<RemoveRoleFromUserCommand, Result>
{
    private readonly ICurrentUserService _currentUser;
    private readonly IHotelBookingDbContext _context;
    private readonly ILogger<RemoveRoleFromUserCommandHandler> _logger;
    
    public RemoveRoleFromUserCommandHandler(ICurrentUserService currentUser, IHotelBookingDbContext context, ILogger<RemoveRoleFromUserCommandHandler> logger)
    {
        _currentUser = currentUser;
        _context = context;
        _logger = logger;
    }

    public async Task<Result> Handle(RemoveRoleFromUserCommand request, CancellationToken cancellationToken)
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

        if (_currentUser.IsModerator() && role.Name != "Manager")
        {
            _logger.LogWarning("Moderator attempted to remove role {RoleName} from user {UserId}", role.Name,
                request.UserId);
            return Result.Fail("Moderators can only remove the 'Manager' role");
        }

        if (_currentUser.GetUserId() == request.UserId && !_currentUser.IsAdmin())
        {
            _logger.LogWarning("User {UserId} attempted to remove their own role {RoleName}", request.UserId, role.Name);
            return Result.Fail("You cannot remove your own role");
        }
        
        if (user.Roles.All(r => r.Id != role.Id))
        {
            _logger.LogWarning("User with ID {UserId} does not have role {RoleName}", request.UserId, role.Name);
            return Result.Fail("User does not have this role");
        }

        user.Roles.Remove(role);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Role {RoleName} removed from user {UserId} by {CurrentUserId}",
            role.Name, request.UserId, _currentUser.GetUserId());
        return Result.Ok();
    }
}