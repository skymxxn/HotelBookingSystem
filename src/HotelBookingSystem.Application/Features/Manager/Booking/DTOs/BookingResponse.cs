using HotelBookingSystem.Domain.Enums;

namespace HotelBookingSystem.Application.Features.Manager.Booking.DTOs;

public class BookingResponse
{
    public Guid BookingId { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public decimal TotalPrice { get; set; }
    public BookingStatus Status { get; set; }
    public string? RejectionReason { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ConfirmedAt { get; set; }
    public DateTime? RejectedAt { get; set; }
    public DateTime? CancelledAt { get; set; }
    public Guid RoomId { get; set; }
    public Guid UserId { get; set; }
}