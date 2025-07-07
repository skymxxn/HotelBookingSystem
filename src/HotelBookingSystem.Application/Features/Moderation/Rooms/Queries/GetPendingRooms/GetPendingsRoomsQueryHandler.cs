using FluentResults;
using HotelBookingSystem.Application.Common.DTOs;
using HotelBookingSystem.Application.Common.DTOs.Rooms;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using HotelBookingSystem.Application.Common.Interfaces.Users;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.Moderation.Rooms.Queries.GetPendingRooms;

public class GetPendingsRoomsQueryHandler : IRequestHandler<GetPendingRoomsQuery, Result<List<RoomResponse>>>
{
    private readonly IHotelBookingDbContext _context;

    public GetPendingsRoomsQueryHandler(IHotelBookingDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<RoomResponse>>> Handle(GetPendingRoomsQuery request, CancellationToken cancellationToken)
    {
        var rooms = await _context.Rooms
            .Where(r => r.HotelId == request.HotelId && !r.IsApproved)
            .ProjectToType<RoomResponse>()
            .ToListAsync(cancellationToken);

        return Result.Ok(rooms);
    }
}