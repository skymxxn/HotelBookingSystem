using FluentResults;
using HotelBookingSystem.Application.Common.DTOs.Bookings;
using MediatR;

namespace HotelBookingSystem.Application.Features.Bookings.Commands.CancelBooking;

public record CancelBookingCommand(
    Guid BookingId,
    string? Reason
) : IRequest<Result<CancelBookingResponse>>;