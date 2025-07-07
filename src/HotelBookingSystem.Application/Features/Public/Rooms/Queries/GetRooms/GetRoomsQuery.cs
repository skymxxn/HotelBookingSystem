using FluentResults;
using HotelBookingSystem.Application.Common.DTOs;
using HotelBookingSystem.Application.Common.DTOs.Rooms;
using MediatR;

namespace HotelBookingSystem.Application.Features.Public.Rooms.Queries.GetRooms;

public record GetRoomsQuery(
    Guid HotelId
) : IRequest<Result<List<RoomResponse>>>;