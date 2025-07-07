using FluentResults;
using HotelBookingSystem.Application.Common.DTOs.Bookings;
using HotelBookingSystem.Application.Common.Interfaces.Access;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.Bookings.Queries.GetBookings;

public class GetBookingsQueryHandler : IRequestHandler<GetBookingsQuery, Result<List<BookingResponse>>>
{
    private readonly IHotelBookingDbContext _context;
    private readonly IAccessService _accessService;

    public GetBookingsQueryHandler(IHotelBookingDbContext context, IAccessService accessService)
    {
        _context = context;
        _accessService = accessService;
    }

    public async Task<Result<List<BookingResponse>>> Handle(GetBookingsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Bookings
            .Include(b => b.Room)
            .ThenInclude(r => r.Hotel)
            .Include(b => b.User)
            .AsQueryable();

        query = _accessService.ApplyBookingAccessFilter(query);
        
        if (request.RoomId.HasValue)
            query = query.Where(b => b.RoomId == request.RoomId.Value);
        
        if (request.HotelId.HasValue)
            query = query.Where(b => b.Room.HotelId == request.HotelId.Value);
        
        if (request.UserId.HasValue)
            query = query.Where(b => b.UserId == request.UserId.Value);

        if (request.MinTotalPrice.HasValue)
            query = query.Where(b => b.TotalPrice >= request.MinTotalPrice);
        
        if (request.MaxTotalPrice.HasValue)
            query = query.Where(b => b.TotalPrice <= request.MaxTotalPrice);
        
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