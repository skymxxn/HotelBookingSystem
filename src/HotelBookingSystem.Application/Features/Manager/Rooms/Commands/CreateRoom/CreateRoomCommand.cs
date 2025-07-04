using FluentResults;
using MediatR;

namespace HotelBookingSystem.Application.Features.Manager.Rooms.Commands.CreateRoom;

public record CreateRoomCommand(
    string Name,
    string Description,
    int Capacity,
    decimal PricePerNight,
    Guid HotelId
    ) : IRequest<Result<Guid>>;