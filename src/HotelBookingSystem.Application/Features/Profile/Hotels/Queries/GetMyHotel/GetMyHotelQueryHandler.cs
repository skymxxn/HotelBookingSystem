using FluentResults;
using HotelBookingSystem.Application.Common.DTOs;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using HotelBookingSystem.Application.Common.Interfaces.Users;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Application.Features.Profile.Hotels.Queries.GetMyHotel;

public class GetMyHotelQueryHandler : IRequestHandler<GetMyHotelQuery, Result<HotelResponse>>
{
    private readonly IHotelBookingDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly ILogger<GetMyHotelQueryHandler> _logger;
    
    public GetMyHotelQueryHandler(IHotelBookingDbContext context, ICurrentUserService currentUser, ILogger<GetMyHotelQueryHandler> logger)
    {
        _context = context;
        _currentUser = currentUser;
        _logger = logger;
    }
    
    public async Task<Result<HotelResponse>> Handle(GetMyHotelQuery request, CancellationToken cancellationToken)
    {
        var managerId = _currentUser.GetUserId();
        var hotel = await _context.Hotels
            .Where(h => h.Id == request.HotelId && h.OwnerId == managerId)
            .ProjectToType<HotelResponse>()
            .FirstOrDefaultAsync(cancellationToken);

        if (hotel == null)
        {
            _logger.LogWarning("Hotel with ID {HotelId} not found for manager with ID {ManagerId}", request.HotelId, managerId);
            return Result.Fail(new Error("Hotel not found."));
        }

        return Result.Ok(hotel);
    }
}