using FluentResults;
using HotelBookingSystem.Application.Common.DTOs;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Application.Features.Hotels.Queries.GetHotel;

public class GetHotelQueryHandler : IRequestHandler<GetHotelQuery, Result<HotelResponse>>
{
    private readonly IHotelBookingDbContext _context;
    private readonly ILogger<GetHotelQueryHandler> _logger;
    
    public GetHotelQueryHandler(IHotelBookingDbContext context, ILogger<GetHotelQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<HotelResponse>> Handle(GetHotelQuery request, CancellationToken cancellationToken)
    {
        var hotel = await _context.Hotels
            .Where(h => h.Id == request.HotelId && h.IsVisible)
            .ProjectToType<HotelResponse>()
            .FirstOrDefaultAsync(cancellationToken);
        
        if (hotel == null)
        {
            _logger.LogWarning("Hotel with ID {HotelId} not found or not visible", request.HotelId);
            return Result.Fail(new Error("Hotel not found or not visible."));
        }
        
        return hotel;
    }
}