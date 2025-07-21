using FluentResults;
using MediatR;

namespace HotelBookingSystem.Application.Features.Bookings.Commands.ConfirmBooking;

public record ConfirmBookingCommand(string Token) : IRequest<Result>;