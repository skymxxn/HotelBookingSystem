using HotelBookingSystem.Application.Common.DTOs;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Hotels.GetPendingHotels;

public class GetPendingHotelsQueryHandler : IRequestHandler<GetPendingHotelsQuery, List<HotelResponse>>
{
    private readonly IHotelBookingDbContext _context;
    
    public GetPendingHotelsQueryHandler(IHotelBookingDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<HotelResponse>> Handle(GetPendingHotelsQuery request, CancellationToken cancellationToken)
    {
        var hotels = await _context.Hotels
            .Where(h => !h.IsApproved)
            .Select(h => new HotelResponse
            {
                Id = h.Id,
                Name = h.Name,
                Description = h.Description,
                Address = h.Address,
                CreatedAt = h.CreatedAt,
                UpdatedAt = h.UpdatedAt,
                IsApproved = h.IsApproved
            }).ToListAsync(cancellationToken);
        
        return hotels;
    }
}