using FluentResults;
using MediatR;

namespace HotelBookingSystem.Application.Features.Manager.Rooms.Commands.DeleteRoom;

public record DeleteRoomCommand(
    Guid RoomId
) : IRequest<Result>;