using FluentResults;
using MediatR;

namespace HotelBookingSystem.Application.Authentication.Logout;

public record LogoutCommand(string RefreshToken) : IRequest<Result>;