using FluentResults;
using MediatR;

namespace HotelBookingSystem.Application.Features.Authentication.Commands.Register;

public sealed record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string PhoneNumber,
    string RoleId
    ) : IRequest<Result<Guid>>;