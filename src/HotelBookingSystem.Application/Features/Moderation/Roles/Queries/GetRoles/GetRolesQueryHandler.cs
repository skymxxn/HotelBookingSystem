using FluentResults;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using HotelBookingSystem.Application.Features.Moderation.Roles.DTOs;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.Moderation.Roles.Queries.GetRoles;

public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, Result<List<RoleResponse>>>
{
    private readonly IHotelBookingDbContext _context;
    
    public GetRolesQueryHandler(IHotelBookingDbContext context)
    {
        _context = context;
    }
    
    public async Task<Result<List<RoleResponse>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _context.Roles
            .ProjectToType<RoleResponse>()
            .ToListAsync(cancellationToken);
        
        return Result.Ok(roles);
    }
}