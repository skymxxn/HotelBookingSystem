using FluentResults;
using HotelBookingSystem.Application.Common.DTOs.Bookings;
using HotelBookingSystem.Domain.Enums;
using MediatR;

namespace HotelBookingSystem.Application.Features.Bookings.Queries.GetBookings;

public record GetBookingsQuery(
    Guid? RoomId,
    Guid? HotelId,
    Guid? UserId,
    decimal? MinTotalPrice,
    decimal? MaxTotalPrice,
    BookingStatus? Status,
    DateTime? FromDate,
    DateTime? ToDate
    ) : IRequest<Result<List<BookingResponse>>>;