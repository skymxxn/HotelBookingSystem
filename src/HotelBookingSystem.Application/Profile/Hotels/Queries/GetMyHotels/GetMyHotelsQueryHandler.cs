using HotelBookingSystem.Application.Common.DTOs;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using HotelBookingSystem.Application.Common.Interfaces.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Profile.Queries.GetMyHotels;

public class GetMyHotelsQueryHandler : IRequestHandler<GetMyHotelsQuery, List<HotelResponse>>
{
    private readonly IHotelBookingDbContext _context;
    private readonly ICurrentUserService _currentUser;
    
    public GetMyHotelsQueryHandler(IHotelBookingDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }
    
    public async Task<List<HotelResponse>> Handle(GetMyHotelsQuery request, CancellationToken cancellationToken)
    {
        var managerId = _currentUser.GetUserId();
        var hotels = await _context.Hotels
            .Where(h => h.OwnerId == managerId)
            .Select(h => new HotelResponse
            {
                Id = h.Id,
                Name = h.Name,
                Description = h.Description,
                Address = h.Address,
                CreatedAt = h.CreatedAt,
                UpdatedAt = h.UpdatedAt,
                IsApproved = h.IsApproved,
                IsVisible = h.IsVisible
            }).ToListAsync(cancellationToken);
        
        return hotels;
    }
}