using FluentResults;
using HotelBookingSystem.Application.Common.DTOs.Rooms;
using HotelBookingSystem.Application.Common.Interfaces.Access;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.Rooms.Queries.GetRooms;

public class GetRoomsQueryHandler : IRequestHandler<GetRoomsQuery, Result<List<RoomResponse>>>
{
    private readonly IHotelBookingDbContext _context;
    private readonly IAccessService _accessService;

    public GetRoomsQueryHandler(IHotelBookingDbContext context, IAccessService accessService)
    {
        _context = context;
        _accessService = accessService;
    }

    public async Task<Result<List<RoomResponse>>> Handle(GetRoomsQuery request, CancellationToken cancellationToken)
    {
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
        
        var rooms = await query
            .ProjectToType<RoomResponse>()
            .ToListAsync(cancellationToken);

        return Result.Ok(rooms);
    }
}