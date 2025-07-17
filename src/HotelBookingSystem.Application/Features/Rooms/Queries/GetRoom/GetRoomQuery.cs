using FluentResults;
using HotelBookingSystem.Application.Common.DTOs.Rooms;
using MediatR;

namespace HotelBookingSystem.Application.Features.Rooms.Queries.GetRoom;

public record GetRoomQuery(
    Guid RoomId
    ) : IRequest<Result<RoomResponse>>;