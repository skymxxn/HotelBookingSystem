using FluentResults;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using HotelBookingSystem.Application.Common.Interfaces.Users;
using HotelBookingSystem.Application.Features.Moderation.Users.DTOs;
using HotelBookingSystem.Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.Moderation.Users.Queries.GetUsers;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, Result<List<UserResponse>>>
{
    private readonly ICurrentUserService _currentUser;
    private readonly IHotelBookingDbContext _context;
    
    public GetUsersQueryHandler(ICurrentUserService currentUser, IHotelBookingDbContext context)
    {
        _currentUser = currentUser;
        _context = context;
    }

    public async Task<Result<List<UserResponse>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        IQueryable<User> query = _context.Users.Include(u => u.Roles);
        
        if (_currentUser.IsModerator() && !_currentUser.IsAdmin())
        {
            query = query.Where(u => u.Roles.All(r => r.Name != "Admin"));
        }
        
        var users = await query.ProjectToType<UserResponse>().ToListAsync(cancellationToken);
        
        return Result.Ok(users);
    }
}