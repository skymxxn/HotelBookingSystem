using FluentResults;
using MediatR;

namespace HotelBookingSystem.Application.Features.Authentication.Logout;

public record LogoutCommand(string RefreshToken) : IRequest<Result>;