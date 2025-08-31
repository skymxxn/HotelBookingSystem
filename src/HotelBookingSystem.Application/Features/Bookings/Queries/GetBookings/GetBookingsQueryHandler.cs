using FluentResults;
using HotelBookingSystem.Application.Common.DTOs.Bookings;
using HotelBookingSystem.Application.Common.Interfaces.Access;
using HotelBookingSystem.Application.Common.Interfaces.Cache;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using HotelBookingSystem.Application.Common.Utils;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.Bookings.Queries.GetBookings;

public class GetBookingsQueryHandler : IRequestHandler<GetBookingsQuery, Result<List<BookingResponse>>>
{
    private readonly IHotelBookingDbContext _context;
    private readonly IAccessService _accessService;
    private readonly IRedisCacheService _cache;

    public GetBookingsQueryHandler(IHotelBookingDbContext context, IAccessService accessService, IRedisCacheService cache)
    {
        _context = context;
        _accessService = accessService;
        _cache = cache;
    }

    public async Task<Result<List<BookingResponse>>> Handle(GetBookingsQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = CacheKeyBuilder.Build("GetBookings",
            request.UserId,
            request.HotelId,
            request.RoomId,
            request.Page,
            request.PageSize,
            request.SortBy,
            request.SortOrder
        );

        var bookings = await _cache.GetDataAsync<List<BookingResponse>>(cacheKey);
        
        if (bookings is not null)
            return Result.Ok(bookings);
        
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

        query = (request.SortBy?.ToLower(), request.SortOrder.ToLower()) switch
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
            
        bookings = await query
            .ProjectToType<BookingResponse>()
            .ToListAsync(cancellationToken);
        
        await _cache.SetDataAsync(cacheKey, bookings);
        
        return Result.Ok(bookings);
    }
}