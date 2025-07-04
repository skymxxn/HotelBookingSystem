using FluentResults;
using HotelBookingSystem.Application.Common.DTOs;
using MediatR;

namespace HotelBookingSystem.Application.Features.Manager.Rooms.Queries.GetRoom;

public record GetRoomQuery(
    Guid HotelId,
    Guid RoomId
) : IRequest<Result<RoomResponse>>;