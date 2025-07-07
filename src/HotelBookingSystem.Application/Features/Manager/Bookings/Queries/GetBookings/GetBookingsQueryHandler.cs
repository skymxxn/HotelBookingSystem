using FluentResults;
using HotelBookingSystem.Application.Common.DTOs.Bookings;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using HotelBookingSystem.Application.Common.Interfaces.Users;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Application.Features.Manager.Bookings.Queries.GetBookings;

public class GetBookingsQueryHandler : IRequestHandler<GetBookingsQuery, Result<List<BookingResponse>>>
{
    private readonly IHotelBookingDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly ILogger<GetBookingsQueryHandler> _logger;

    public GetBookingsQueryHandler(
        IHotelBookingDbContext context,
        ICurrentUserService currentUser,
        ILogger<GetBookingsQueryHandler> logger)
    {
        _context = context;
        _currentUser = currentUser;
        _logger = logger;
    }
    
    public async Task<Result<List<BookingResponse>>> Handle(GetBookingsQuery request, CancellationToken cancellationToken)
    {
        var managerId = _currentUser.GetUserId();
        
        var query = _context.Bookings
            .Where(b => b.Room.Hotel.OwnerId == managerId)
            .AsQueryable();

        if (request.RoomId.HasValue)
            query = query.Where(b => b.Room.Id == request.RoomId);
        
        if (request.UserId.HasValue)
            query = query.Where(b => b.UserId == request.UserId.Value);
        
        if (request.Status.HasValue)
            query = query.Where(b => b.Status == request.Status);
        
        if (request.FromDate.HasValue)
            query = query.Where(b => b.FromDate >= request.FromDate.Value);
        
        if (request.ToDate.HasValue)
            query = query.Where(b => b.ToDate <= request.ToDate.Value);
        
        var bookings = await query
            .ProjectToType<BookingResponse>()
            .ToListAsync(cancellationToken);
        
        return Result.Ok(bookings);
    }
}