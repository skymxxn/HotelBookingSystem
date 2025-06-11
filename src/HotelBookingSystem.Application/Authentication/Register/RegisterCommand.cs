using FluentResults;
using MediatR;

namespace HotelBookingSystem.Application.Authentication.Register;

public sealed record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string PhoneNumber
    ) : IRequest<Result<Guid>>;