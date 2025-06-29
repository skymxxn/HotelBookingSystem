using FluentResults;
using HotelBookingSystem.Application.Common.DTOs;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.Moderation.Hotels.Queries.GetPendingHotels;

public class GetPendingHotelsQueryHandler : IRequestHandler<GetPendingHotelsQuery, Result<List<HotelResponse>>>
{
    private readonly IHotelBookingDbContext _context;
    
    public GetPendingHotelsQueryHandler(IHotelBookingDbContext context)
    {
        _context = context;
    }
    
    public async Task<Result<List<HotelResponse>>> Handle(GetPendingHotelsQuery request, CancellationToken cancellationToken)
    {
        var hotels = await _context.Hotels
            .Where(h => !h.IsApproved)
            .ProjectToType<HotelResponse>()
            .ToListAsync(cancellationToken);
        
        return Result.Ok(hotels);
    }
}