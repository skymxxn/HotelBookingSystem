using FluentResults;
using MediatR;

namespace HotelBookingSystem.Application.Features.Authentication.Commands.VerifyEmail;

public record VerifyEmailCommand(string Token) :  IRequest<Result>;