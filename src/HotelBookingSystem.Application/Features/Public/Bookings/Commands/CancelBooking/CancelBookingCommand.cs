using FluentResults;
using HotelBookingSystem.Application.Common.DTOs.Bookings;
using MediatR;

namespace HotelBookingSystem.Application.Features.Public.Bookings.Commands.CancelBooking;

public record CancelBookingCommand(
    Guid BookingId,
    string? Reason
) : IRequest<Result<CancelBookingResponse>>;