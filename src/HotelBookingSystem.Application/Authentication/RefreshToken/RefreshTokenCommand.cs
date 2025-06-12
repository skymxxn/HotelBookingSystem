using FluentResults;
using HotelBookingSystem.Application.Authentication.Common;
using MediatR;

namespace HotelBookingSystem.Application.Authentication.RefreshToken;

public record RefreshTokenCommand(string RefreshToken) : IRequest<Result<AuthResultDto>>;