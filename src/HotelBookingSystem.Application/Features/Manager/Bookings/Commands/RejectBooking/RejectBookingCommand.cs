using FluentResults;
using HotelBookingSystem.Application.Common.DTOs.Bookings;
using MediatR;

namespace HotelBookingSystem.Application.Features.Manager.Bookings.Commands.RejectBooking;

public record RejectBookingCommand(Guid BookingId, string Reason) : IRequest<Result<RejectBookingResponse>>;