using FluentResults;
using HotelBookingSystem.Application.Common.DTOs.Bookings;
using MediatR;

namespace HotelBookingSystem.Application.Features.Bookings.Queries.GetBooking;

public record GetBookingQuery(
    Guid BookingId
) : IRequest<Result<BookingResponse>>;