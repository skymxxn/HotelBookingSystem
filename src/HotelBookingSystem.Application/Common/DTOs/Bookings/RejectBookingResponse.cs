namespace HotelBookingSystem.Application.Common.DTOs.Bookings;

public class RejectBookingResponse
{
    public Guid Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime RejectedAt { get; set; }
    public string? RejectionReason { get; set; }
}