using FluentResults;
using HotelBookingSystem.Application.Common.DTOs;
using MediatR;

namespace HotelBookingSystem.Application.Features.Moderation.Rooms.Queries.GetPendingRoom;

public record GetPendingRoomQuery(
    Guid HotelId,
    Guid RoomId
) : IRequest<Result<RoomResponse>>;