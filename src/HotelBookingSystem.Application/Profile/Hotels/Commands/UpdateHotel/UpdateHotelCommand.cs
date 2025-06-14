using FluentResults;
using MediatR;

namespace HotelBookingSystem.Application.Profile.Commands.UpdateHotel;

public record UpdateHotelCommand(
    Guid Id,
    string Name,
    string Description,
    string Address
) : IRequest<Result>;