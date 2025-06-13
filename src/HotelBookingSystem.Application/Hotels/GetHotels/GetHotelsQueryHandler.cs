using HotelBookingSystem.Application.Common.DTOs;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Hotels.GetHotels;

public class GetHotelsQueryHandler : IRequestHandler<GetHotelsQuery, List<HotelResponse>>
{
    private readonly IHotelBookingDbContext _context;
    
    public GetHotelsQueryHandler(IHotelBookingDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<HotelResponse>> Handle(GetHotelsQuery request, CancellationToken cancellationToken)
    {
        var hotels = await _context.Hotels
            .Where(h => h.IsApproved)
            .Select(h => new HotelResponse
            {
                Id = h.Id,
                Name = h.Name,
                Description = h.Description,
                Address = h.Address,
                CreatedAt = h.CreatedAt,
                UpdatedAt = h.UpdatedAt,
                IsApproved = h.IsApproved
            })
            .ToListAsync(cancellationToken);

        return hotels;
    }
}