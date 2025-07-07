using HotelBookingSystem.Application.Common.DTOs.Rooms;
using HotelBookingSystem.Application.Common.DTOs.Users;

namespace HotelBookingSystem.Application.Common.DTOs.Bookings;

public class BookingResponse
{
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? RejectionReason { get; set; }
    public string? CancellationReason { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ConfirmedAt { get; set; }
    public DateTime? RejectedAt { get; set; }
    public DateTime? CancelledAt { get; set; }

    public UserDetailsDto UserDetails { get; set; } = new();
    public RoomsDetailsDto RoomDetails { get; set; } = new();
}