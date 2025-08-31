using FluentResults;
using HotelBookingSystem.Application.Common.DTOs.Hotels;
using HotelBookingSystem.Application.Common.Interfaces.Access;
using HotelBookingSystem.Application.Common.Interfaces.Cache;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using HotelBookingSystem.Application.Common.Utils;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.Hotels.Queries.GetHotels;

public class GetHotelsQueryHandler : IRequestHandler<GetHotelsQuery, Result<List<HotelResponse>>>
{
    private readonly IHotelBookingDbContext _context;
    private readonly IAccessService _accessService;
    private readonly IRedisCacheService _cache;

    public GetHotelsQueryHandler(IHotelBookingDbContext context, IAccessService accessService, IRedisCacheService cache)
    {
        _context = context;
        _accessService = accessService;
        _cache = cache;
    }
    
    public async Task<Result<List<HotelResponse>>> Handle(GetHotelsQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = CacheKeyBuilder.Build("GetHotels",
            request.Name,
            request.Address,
            request.IsApproved,
            request.IsPublished,
            request.CreatedAt,
            request.UpdatedAt,
            request.OwnerId,
            request.Page,
            request.PageSize,
            request.SortBy,
            request.SortOrder
        );
        
        var hotels = await _cache.GetDataAsync<List<HotelResponse>>(cacheKey);

        if (hotels is not null)
            return Result.Ok(hotels);
        
        var query = _context.Hotels.AsQueryable();

        query = _accessService.ApplyHotelAccessFilter(query);
        
        if (!string.IsNullOrWhiteSpace(request.Name))
            query = query.Where(h => h.Name.ToLower().Contains(request.Name.ToLower()));
        
        if (!string.IsNullOrWhiteSpace(request.Address))
            query = query.Where(h => h.Address.ToLower().Contains(request.Address.ToLower()));
        
        if (request.IsApproved.HasValue)
            query = query.Where(h => h.IsApproved == request.IsApproved.Value);
        
        if (request.IsPublished.HasValue)
            query = query.Where(h => h.IsPublished == request.IsPublished.Value);
        
        if (request.CreatedAt.HasValue)
            query = query.Where(h => h.CreatedAt >= request.CreatedAt.Value);
        
        if (request.UpdatedAt.HasValue)
            query = query.Where(h => h.UpdatedAt >= request.UpdatedAt.Value);
        
        if (request.OwnerId.HasValue)
            query = query.Where(h => h.OwnerId == request.OwnerId.Value);
        
        query = (request.SortBy?.ToLower(), request.SortOrder.ToLower()) switch
        {
            ("name", "asc") => query.OrderBy(h => h.Name),
            ("name", "desc") => query.OrderByDescending(h => h.Name),
            ("address", "asc") => query.OrderBy(h => h.Address),
            ("address", "desc") => query.OrderByDescending(h => h.Address),
            ("isapproved", "asc") => query.OrderBy(h => h.IsApproved),
            ("isapproved", "desc") => query.OrderByDescending(h => h.IsApproved),
            ("ispublished", "asc") => query.OrderBy(h => h.IsPublished),
            ("ispublished", "desc") => query.OrderByDescending(h => h.IsPublished),
            ("createdat", "asc") => query.OrderBy(h => h.CreatedAt),
            ("createdat", "desc") => query.OrderByDescending(h => h.CreatedAt),
            ("updatedat", "asc") => query.OrderBy(h => h.UpdatedAt),
            ("updatedat", "desc") => query.OrderByDescending(h => h.UpdatedAt),
            _ => query.OrderByDescending(h => h.CreatedAt)
        };
        
        var skip = (request.Page - 1) * request.PageSize;
        query = query.Skip(skip).Take(request.PageSize);
        
        hotels = await query
            .ProjectToType<HotelResponse>()
            .ToListAsync(cancellationToken);

        await _cache.SetDataAsync(cacheKey, hotels);

        return Result.Ok(hotels);
    }
}