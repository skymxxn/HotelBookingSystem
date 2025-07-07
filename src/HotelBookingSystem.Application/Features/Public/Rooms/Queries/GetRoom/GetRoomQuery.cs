using FluentResults;
using HotelBookingSystem.Application.Common.DTOs;
using HotelBookingSystem.Application.Common.DTOs.Rooms;
using MediatR;

namespace HotelBookingSystem.Application.Features.Public.Rooms.Queries.GetRoom;

public record GetRoomQuery(
    Guid HotelId,
    Guid RoomId
    ) : IRequest<Result<RoomResponse>>;