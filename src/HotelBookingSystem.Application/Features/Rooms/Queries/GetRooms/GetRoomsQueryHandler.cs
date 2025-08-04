using FluentResults;
using HotelBookingSystem.Application.Common.DTOs.Rooms;
using HotelBookingSystem.Application.Common.Interfaces.Access;
using HotelBookingSystem.Application.Common.Interfaces.Cache;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.Rooms.Queries.GetRooms;

public class GetRoomsQueryHandler : IRequestHandler<GetRoomsQuery, Result<List<RoomResponse>>>
{
    private readonly IHotelBookingDbContext _context;
    private readonly IAccessService _accessService;
    private readonly ICacheService _cacheService;

    public GetRoomsQueryHandler(IHotelBookingDbContext context, IAccessService accessService, ICacheService cacheService)
    {
        _context = context;
        _accessService = accessService;
        _cacheService = cacheService;
    }

    public async Task<Result<List<RoomResponse>>> Handle(GetRoomsQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"GetRooms_{request.Name}_{request.Capacity}_{request.MinPrice}_{request.MaxPrice}_{request.IsApproved}_{request.IsPublished}_{request.CreatedAt}_{request.UpdatedAt}_{request.HotelId}_{request.Page}_{request.PageSize}_{request.SortBy}_{request.SortOrder}";

        var cached = await _cacheService.GetAsync<List<RoomResponse>>(cacheKey);

        if (cached is not null)
            return Result.Ok(cached);

        var query = _context.Rooms.AsQueryable();

        query = _accessService.ApplyRoomAccessFilter(query);

        if (!string.IsNullOrWhiteSpace(request.Name))
            query = query.Where(r => r.Name.Contains(request.Name));

        if (request.Capacity.HasValue)
            query = query.Where(r => r.Capacity >= request.Capacity.Value);
        
        if (request.MinPrice.HasValue)
            query = query.Where(r => r.PricePerNight >= request.MinPrice.Value);
        
        if  (request.MaxPrice.HasValue)
            query = query.Where(r => r.PricePerNight <= request.MaxPrice.Value);
        
        if (request.IsApproved.HasValue)
            query = query.Where(r => r.IsApproved == request.IsApproved.Value);
        
        if (request.IsPublished.HasValue)
            query = query.Where(r => r.IsPublished == request.IsPublished.Value);
        
        if (request.CreatedAt.HasValue)
            query = query.Where(r => r.CreatedAt >= request.CreatedAt.Value);
        
        if (request.UpdatedAt.HasValue)
            query = query.Where(r => r.UpdatedAt >= request.UpdatedAt.Value);
        
        if (request.HotelId.HasValue)
            query = query.Where(r => r.HotelId == request.HotelId.Value);
        
        query = (request.SortBy?.ToLower(), request.SortOrder?.ToLower()) switch
        {
            ("name", "asc") => query.OrderBy(r => r.Name),
            ("name", "desc") => query.OrderByDescending(r => r.Name),
            ("capacity", "asc") => query.OrderBy(r => r.Capacity),
            ("capacity", "desc") => query.OrderByDescending(r => r.Capacity),
            ("price", "asc") => query.OrderBy(r => r.PricePerNight),
            ("price", "desc") => query.OrderByDescending(r => r.PricePerNight),
            ("isapproved", "asc") => query.OrderBy(r => r.IsApproved),
            ("isapproved", "desc") => query.OrderByDescending(r => r.IsApproved),
            ("ispublished", "asc") => query.OrderBy(r => r.IsPublished),
            ("ispublished", "desc") => query.OrderByDescending(r => r.IsPublished),
            ("createdat", "asc") => query.OrderBy(r => r.CreatedAt),
            ("createdat", "desc") => query.OrderByDescending(r => r.CreatedAt),
            ("updatedat", "asc") => query.OrderBy(r => r.UpdatedAt),
            ("updatedat", "desc") => query.OrderByDescending(r => r.UpdatedAt),
            _ => query.OrderByDescending(r => r.CreatedAt)
        };
        
        var skip = (request.Page - 1) * request.PageSize;
        query = query.Skip(skip).Take(request.PageSize);
        
        var rooms = await query
            .ProjectToType<RoomResponse>()
            .ToListAsync(cancellationToken);

        await _cacheService.SetAsync(cacheKey, rooms, TimeSpan.FromMinutes(5));

        return Result.Ok(rooms);
    }
}