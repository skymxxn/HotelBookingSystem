using FluentResults;
using MediatR;

namespace HotelBookingSystem.Application.Features.Bookings.Commands.UpdateBooking;

public record UpdateBookingCommand(
    Guid BookingId,
    DateTime FromDate,
    DateTime ToDate,
    string? Description
) : IRequest<Result>;
