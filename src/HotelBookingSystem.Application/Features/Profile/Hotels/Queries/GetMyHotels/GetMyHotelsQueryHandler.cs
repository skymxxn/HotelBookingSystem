using FluentResults;
using HotelBookingSystem.Application.Common.DTOs;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using HotelBookingSystem.Application.Common.Interfaces.Users;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.Profile.Hotels.Queries.GetMyHotels;

public class GetMyHotelsQueryHandler : IRequestHandler<GetMyHotelsQuery, Result<List<HotelResponse>>>
{
    private readonly IHotelBookingDbContext _context;
    private readonly ICurrentUserService _currentUser;
    
    public GetMyHotelsQueryHandler(IHotelBookingDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }
    
    public async Task<Result<List<HotelResponse>>> Handle(GetMyHotelsQuery request, CancellationToken cancellationToken)
    {
        var managerId = _currentUser.GetUserId();
        var hotels = await _context.Hotels
            .Where(h => h.OwnerId == managerId)
            .ProjectToType<HotelResponse>()
            .ToListAsync(cancellationToken);
        
        return Result.Ok(hotels);
    }
}