using FluentResults;
using HotelBookingSystem.Application.Common.DTOs.Bookings;
using MediatR;

namespace HotelBookingSystem.Application.Features.Bookings.Commands.CreateBooking;

public record CreateBookingCommand(
    Guid RoomId,
    DateTime FromDate,
    DateTime ToDate,
    string? Description
    ) : IRequest<Result<CreateBookingResponse>>;