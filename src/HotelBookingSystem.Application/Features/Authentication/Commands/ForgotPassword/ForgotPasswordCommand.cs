using FluentResults;
using MediatR;

namespace HotelBookingSystem.Application.Features.Authentication.Commands.ForgotPassword;

public record ForgotPasswordCommand(string Email) :  IRequest<Result>;