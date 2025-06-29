using FluentResults;
using HotelBookingSystem.Application.Common.DTOs;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Application.Features.Moderation.Hotels.Queries.GetPendingHotel;

public class GetPendingHotelQueryHandler : IRequestHandler<GetPendingHotelQuery, Result<HotelResponse>>
{
    private readonly IHotelBookingDbContext _context;
    private readonly ILogger<GetPendingHotelQueryHandler> _logger;
    
    public GetPendingHotelQueryHandler(IHotelBookingDbContext context, ILogger<GetPendingHotelQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<HotelResponse>> Handle(GetPendingHotelQuery request, CancellationToken cancellationToken)
    {
        var hotel = await _context.Hotels
            .Where(h => h.Id == request.HotelId && !h.IsApproved)
            .ProjectToType<HotelResponse>()
            .FirstOrDefaultAsync(cancellationToken);

        if (hotel == null)
        {
            _logger.LogWarning("Pending hotel with ID {HotelId} not found", request.HotelId);
            return Result.Fail(new Error("Pending hotel not found."));
        }
        
        return Result.Ok(hotel);
    }
}