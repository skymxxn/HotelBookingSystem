using FluentResults;
using HotelBookingSystem.Application.Common.DTOs;
using HotelBookingSystem.Application.Common.DTOs.Rooms;
using MediatR;

namespace HotelBookingSystem.Application.Features.Moderation.Rooms.Queries.GetPendingRoom;

public record GetPendingRoomQuery(
    Guid HotelId,
    Guid RoomId
) : IRequest<Result<RoomResponse>>;