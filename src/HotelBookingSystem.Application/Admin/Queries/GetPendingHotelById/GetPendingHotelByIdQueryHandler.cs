using HotelBookingSystem.Application.Common.DTOs;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Admin.Queries.GetPendingHotelById;

public class GetPendingHotelByIdQueryHandler : IRequestHandler<GetPendingHotelByIdQuery, HotelResponse?>
{
    private readonly IHotelBookingDbContext _context;
    
    public GetPendingHotelByIdQueryHandler(IHotelBookingDbContext context)
    {
        _context = context;
    }

    public async Task<HotelResponse?> Handle(GetPendingHotelByIdQuery request, CancellationToken cancellationToken)
    {
        var hotel = await _context.Hotels
            .Where(h => h.Id == request.HotelId && !h.IsApproved)
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