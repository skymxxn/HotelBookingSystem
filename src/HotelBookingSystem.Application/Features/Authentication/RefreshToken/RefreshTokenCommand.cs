using FluentResults;
using HotelBookingSystem.Application.Features.Authentication.Common;
using MediatR;

namespace HotelBookingSystem.Application.Features.Authentication.RefreshToken;

public record RefreshTokenCommand(string RefreshToken) : IRequest<Result<AuthResultDto>>;