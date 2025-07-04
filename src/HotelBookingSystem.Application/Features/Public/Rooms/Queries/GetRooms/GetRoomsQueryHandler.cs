using FluentResults;
using HotelBookingSystem.Application.Common.DTOs;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.Public.Rooms.Queries.GetRooms;

public class GetRoomsQueryHandler : IRequestHandler<GetRoomsQuery, Result<List<RoomResponse>>>
{
    private readonly IHotelBookingDbContext _context;

    public GetRoomsQueryHandler(IHotelBookingDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<RoomResponse>>> Handle(GetRoomsQuery request, CancellationToken cancellationToken)
    {
        var rooms = await _context.Rooms
            .Where(r => r.HotelId == request.HotelId && r.IsPublished && r.Hotel.IsPublished)
            .ProjectToType<RoomResponse>()
            .ToListAsync(cancellationToken);

        return Result.Ok(rooms);
    }
}