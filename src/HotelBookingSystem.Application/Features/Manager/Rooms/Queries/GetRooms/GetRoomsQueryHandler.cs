using FluentResults;
using HotelBookingSystem.Application.Common.DTOs;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using HotelBookingSystem.Application.Common.Interfaces.Users;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.Manager.Rooms.Queries.GetRooms;

public class GetRoomsQueryHandler : IRequestHandler<GetRoomsQuery, Result<List<RoomResponse>>>
{
    private readonly IHotelBookingDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public GetRoomsQueryHandler(IHotelBookingDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Result<List<RoomResponse>>> Handle(GetRoomsQuery request, CancellationToken cancellationToken)
    {
        var managerId = _currentUser.GetUserId();
        var rooms = await _context.Rooms
            .Where(r => r.HotelId == request.HotelId && r.Hotel.OwnerId == managerId)
            .ProjectToType<RoomResponse>()
            .ToListAsync(cancellationToken);

        return Result.Ok(rooms);
    }
}