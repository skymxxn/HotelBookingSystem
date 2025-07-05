using FluentResults;
using HotelBookingSystem.Application.Features.Manager.Booking.DTOs;
using HotelBookingSystem.Domain.Enums;
using MediatR;

namespace HotelBookingSystem.Application.Features.Manager.Booking.Queries.GetBookings;

public class GetBookingsQuery : IRequest<Result<List<BookingResponse>>>
{
    public Guid? RoomId { get; set; }
    public BookingStatus? Status { get; set; } 
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; } 
}