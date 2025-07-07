using FluentResults;
using HotelBookingSystem.Application.Common.DTOs.Bookings;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using HotelBookingSystem.Application.Common.Interfaces.Users;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.Public.Bookings.Queries.GetBookings;

public class GetBookingsQueryHandler : IRequestHandler<GetBookingsQuery, Result<List<BookingResponse>>>
{
    private readonly IHotelBookingDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public GetBookingsQueryHandler(IHotelBookingDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Result<List<BookingResponse>>> Handle(GetBookingsQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.GetUserId();
        
        var query = _context.Bookings
            .Include(b => b.Room)
            .ThenInclude(r => r.Hotel)
            .Include(b => b.User)
            .Where(b => b.UserId == userId)
            .AsQueryable();
        
        if (request.RoomId.HasValue)
            query = query.Where(b => b.RoomId == request.RoomId.Value);
        
        if (request.Status.HasValue)
            query = query.Where(b => b.Status == request.Status.Value);
        
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