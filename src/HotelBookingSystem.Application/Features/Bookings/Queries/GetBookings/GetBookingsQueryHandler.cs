using FluentResults;
using HotelBookingSystem.Application.Common.DTOs.Bookings;
using HotelBookingSystem.Application.Common.Interfaces.Access;
using HotelBookingSystem.Application.Common.Interfaces.Cache;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.Bookings.Queries.GetBookings;

public class GetBookingsQueryHandler : IRequestHandler<GetBookingsQuery, Result<List<BookingResponse>>>
{
    private readonly IHotelBookingDbContext _context;
    private readonly IAccessService _accessService;
    private readonly ICacheService _cacheService;

    public GetBookingsQueryHandler(IHotelBookingDbContext context, IAccessService accessService, ICacheService cacheService)
    {
        _context = context;
        _accessService = accessService;
        _cacheService = cacheService;
    }

    public async Task<Result<List<BookingResponse>>> Handle(GetBookingsQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"GetBookings_{request.RoomId}_{request.HotelId}_{request.UserId}_{request.MinTotalPrice}_{request.MaxTotalPrice}_{request.Status}_{request.FromDate}_{request.ToDate}_{request.Page}_{request.PageSize}_{request.SortBy}_{request.SortOrder}";

        var cached = await _cacheService.GetAsync<List<BookingResponse>>(cacheKey);
        
        if (cached is not null)
            return Result.Ok(cached);

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

        query = (request.SortBy?.ToLower(), request.SortOrder?.ToLower()) switch
        {
            ("fromdate", "asc") => query.OrderBy(b => b.FromDate),
            ("fromdate", "desc") => query.OrderByDescending(b => b.FromDate),
            ("todate", "asc") => query.OrderBy(b => b.ToDate),
            ("todate", "desc") => query.OrderByDescending(b => b.ToDate),
            ("totalprice", "asc") => query.OrderBy(b => b.TotalPrice),
            ("totalprice", "desc") => query.OrderByDescending(b => b.TotalPrice),
            _ => query.OrderByDescending(b => b.CreatedAt)
        };
        
        var skip = (request.Page - 1) * request.PageSize;
        query = query.Skip(skip).Take(request.PageSize);
            
        var bookings = await query
            .ProjectToType<BookingResponse>()
            .ToListAsync(cancellationToken);
        
        return Result.Ok(bookings);
    }
}