using HotelBookingSystem.Application.Common.DTOs;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.Hotels.Queries.GetHotelById;

public class GetHotelByIdQueryHandler : IRequestHandler<GetHotelByIdQuery, HotelResponse?>
{
    private readonly IHotelBookingDbContext _context;
    
    public GetHotelByIdQueryHandler(IHotelBookingDbContext context)
    {
        _context = context;
    }

    public async Task<HotelResponse?> Handle(GetHotelByIdQuery request, CancellationToken cancellationToken)
    {
        var hotel = await _context.Hotels
            .Where(h => h.Id == request.HotelId && h.IsVisible)
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