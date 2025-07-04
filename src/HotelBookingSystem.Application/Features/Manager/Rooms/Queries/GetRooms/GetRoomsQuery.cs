using FluentResults;
using HotelBookingSystem.Application.Common.DTOs;
using MediatR;

namespace HotelBookingSystem.Application.Features.Manager.Rooms.Queries.GetRooms;

public record GetRoomsQuery(
    Guid HotelId
) : IRequest<Result<List<RoomResponse>>>;