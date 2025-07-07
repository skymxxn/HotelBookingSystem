using FluentResults;
using MediatR;

namespace HotelBookingSystem.Application.Features.Moderation.Rooms.Commands.ApproveRoom;

public record ApproveRoomCommand(
    Guid RoomId
) : IRequest<Result<Guid>>;