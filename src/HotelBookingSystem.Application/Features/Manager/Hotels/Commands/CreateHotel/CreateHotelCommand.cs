using FluentResults;
using MediatR;

namespace HotelBookingSystem.Application.Features.Manager.Hotels.Commands.CreateHotel;

public record CreateHotelCommand(
    string Name,
    string Description,
    string Address
    ) : IRequest<Result<Guid>>;