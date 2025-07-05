using FluentResults;
using HotelBookingSystem.Application.Features.Manager.Booking.DTOs;
using MediatR;

namespace HotelBookingSystem.Application.Features.Manager.Booking.Commands.ConfirmBooking;

public record ConfirmBookingCommand(Guid BookingId) : IRequest<Result<ConfirmBookingResponse>>;