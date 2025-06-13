using HotelBookingSystem.Application.Common.DTOs;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Hotels.GetMyHotels;

public class GetMyHotelsQueryHandler : IRequestHandler<GetMyHotelsQuery, List<HotelResponse>>
{
    private readonly IHotelBookingDbContext _context;
    
    public GetMyHotelsQueryHandler(IHotelBookingDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<HotelResponse>> Handle(GetMyHotelsQuery request, CancellationToken cancellationToken)
    {
        var hotels = await _context.Hotels
            .Where(h => h.OwnerId == request.OwnerId)
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