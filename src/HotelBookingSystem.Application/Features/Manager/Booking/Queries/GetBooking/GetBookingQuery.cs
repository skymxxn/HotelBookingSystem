using FluentResults;
using HotelBookingSystem.Application.Features.Manager.Booking.DTOs;
using MediatR;

namespace HotelBookingSystem.Application.Features.Manager.Booking.Queries.GetBooking;

public record GetBookingQuery(Guid BookingId) : IRequest<Result<BookingResponse>>;