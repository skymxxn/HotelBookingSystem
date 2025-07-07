using FluentResults;
using HotelBookingSystem.Application.Common.DTOs.Bookings;
using MediatR;

namespace HotelBookingSystem.Application.Features.Manager.Bookings.Commands.ConfirmBooking;

public record ConfirmBookingCommand(Guid BookingId) : IRequest<Result<ConfirmBookingResponse>>;