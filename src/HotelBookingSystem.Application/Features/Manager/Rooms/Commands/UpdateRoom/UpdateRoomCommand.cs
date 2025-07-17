using FluentResults;
using MediatR;

namespace HotelBookingSystem.Application.Features.Manager.Rooms.Commands.UpdateRoom;

public record UpdateRoomCommand(
    string Name,
    string Description,
    int Capacity,
    decimal PricePerNight,
    Guid RoomId
) : IRequest<Result>;