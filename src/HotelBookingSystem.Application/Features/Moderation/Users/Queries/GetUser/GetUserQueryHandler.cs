using FluentResults;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using HotelBookingSystem.Application.Common.Interfaces.Users;
using HotelBookingSystem.Application.Features.Moderation.Users.DTOs;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Application.Features.Moderation.Users.Queries.GetUser;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, Result<UserResponse>>
{
    private readonly IHotelBookingDbContext _context;
    private readonly ILogger<GetUserQueryHandler> _logger;
    private readonly ICurrentUserService _currentUser;

    public GetUserQueryHandler( ILogger<GetUserQueryHandler> logger, IHotelBookingDbContext context, ICurrentUserService currentUser)
    {
        _logger = logger;
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Result<UserResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Users
            .Include(u => u.Roles)
            .Where(u => u.Id == request.UserId);
        
        var entity = await query.FirstOrDefaultAsync(cancellationToken);
        
        if (entity == null)
        {
            _logger.LogWarning("User with ID {UserId} not found", request.UserId);
            return Result.Fail(new Error("User not found."));
        }
        
        if (_currentUser.IsModerator() && entity.Roles.Any(r => r.Name == "Admin"))
        {
            _logger.LogWarning("Moderator attempted to access admin user {UserId}", request.UserId);
            return Result.Fail(new Error("User not found.")); // специально не выдаём инфу
        }
        
        var user = entity.Adapt<UserResponse>();
        
        return Result.Ok(user);
    }
}