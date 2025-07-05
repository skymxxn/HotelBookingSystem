using FluentResults;
using HotelBookingSystem.Application.Features.Manager.Booking.DTOs;
using MediatR;

namespace HotelBookingSystem.Application.Features.Manager.Booking.Commands.RejectBooking;

public record RejectBookingCommand(Guid BookingId, string Reason) : IRequest<Result<RejectBookingResponse>>;