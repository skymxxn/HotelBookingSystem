using FluentResults;
using MediatR;

namespace HotelBookingSystem.Application.Features.Authentication.Commands.ResetPassword;

public record ResetPasswordCommand(string Token, string NewPassword) : IRequest<Result>;