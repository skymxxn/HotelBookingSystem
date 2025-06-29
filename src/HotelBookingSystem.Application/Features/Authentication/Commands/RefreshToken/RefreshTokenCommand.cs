using FluentResults;
using HotelBookingSystem.Application.Features.Authentication.DTOs;
using MediatR;

namespace HotelBookingSystem.Application.Features.Authentication.Commands.RefreshToken;

public record RefreshTokenCommand(string RefreshToken) : IRequest<Result<AuthResultDto>>;