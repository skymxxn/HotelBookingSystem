using FluentResults;
using MediatR;

namespace HotelBookingSystem.Application.Features.Authentication.Commands.Logout;

public record LogoutCommand(string RefreshToken) : IRequest<Result>;