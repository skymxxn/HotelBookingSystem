namespace HotelBookingSystem.Application.Common.DTOs.Bookings;

public class CancelBookingResponse
{
    public Guid Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CancelledAt { get; set; }
    public string? CancellationReason { get; set; }
}