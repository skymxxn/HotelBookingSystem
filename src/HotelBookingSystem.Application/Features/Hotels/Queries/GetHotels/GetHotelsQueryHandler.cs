using FluentResults;
using HotelBookingSystem.Application.Common.DTOs.Hotels;
using HotelBookingSystem.Application.Common.Interfaces.Access;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.Hotels.Queries.GetHotels;

public class GetHotelsQueryHandler : IRequestHandler<GetHotelsQuery, Result<List<HotelResponse>>>
{
    private readonly IHotelBookingDbContext _context;
    private readonly IAccessService _accessService;
    
    public GetHotelsQueryHandler(IHotelBookingDbContext context, IAccessService accessService)
    {
        _context = context;
        _accessService = accessService;
    }
    
    public async Task<Result<List<HotelResponse>>> Handle(GetHotelsQuery request, CancellationToken cancellationToken)
    {
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
        
        var hotels = await query
            .ProjectToType<HotelResponse>()
            .ToListAsync(cancellationToken);

        return Result.Ok(hotels);
    }
}