using HotelBookingSystem.Application.Common.DTOs;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using HotelBookingSystem.Application.Common.Interfaces.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Profile.Queries.GetMyHotelById;

public class GetMyHotelByIdQueryHandler : IRequestHandler<GetMyHotelByIdQuery, HotelResponse?>
{
    private readonly IHotelBookingDbContext _context;
    private readonly ICurrentUserService _currentUser;
    
    public GetMyHotelByIdQueryHandler(IHotelBookingDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }
    
    public async Task<HotelResponse?> Handle(GetMyHotelByIdQuery request, CancellationToken cancellationToken)
    {
        var managerId = _currentUser.GetUserId();
        var hotel = await _context.Hotels
            .Where(h => h.Id == request.HotelId && h.OwnerId == managerId)
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
            })
            .FirstOrDefaultAsync(cancellationToken);
        
        return hotel;
    }
}