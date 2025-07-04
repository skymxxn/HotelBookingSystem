using FluentResults;
using HotelBookingSystem.Application.Common.DTOs;
using MediatR;

namespace HotelBookingSystem.Application.Features.Moderation.Rooms.Queries.GetPendingRooms;

public record GetPendingRoomsQuery(
    Guid HotelId
) : IRequest<Result<List<RoomResponse>>>;