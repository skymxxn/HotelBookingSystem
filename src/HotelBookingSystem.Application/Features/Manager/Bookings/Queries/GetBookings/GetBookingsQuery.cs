using FluentResults;
using HotelBookingSystem.Application.Common.DTOs.Bookings;
using HotelBookingSystem.Domain.Enums;
using MediatR;

namespace HotelBookingSystem.Application.Features.Manager.Bookings.Queries.GetBookings;

public class GetBookingsQuery : IRequest<Result<List<BookingResponse>>>
{
    public Guid? RoomId { get; set; }
    public Guid? UserId { get; set; }
    public BookingStatus? Status { get; set; } 
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; } 
}