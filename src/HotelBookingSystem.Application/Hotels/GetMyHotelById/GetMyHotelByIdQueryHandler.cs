using HotelBookingSystem.Application.Common.DTOs;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Hotels.GetMyHotelById;

public class GetMyHotelByIdQueryHandler : IRequestHandler<GetMyHotelByIdQuery, HotelResponse?>
{
    private readonly IHotelBookingDbContext _context;
    
    public GetMyHotelByIdQueryHandler(IHotelBookingDbContext context)
    {
        _context = context;
    }
    
    public async Task<HotelResponse?> Handle(GetMyHotelByIdQuery request, CancellationToken cancellationToken)
    {
        var hotel = await _context.Hotels
            .Where(h => h.Id == request.HotelId && h.OwnerId == request.OwnerId)
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
            .FirstOrDefaultAsync(cancellationToken);
        
        return hotel;
    }
}