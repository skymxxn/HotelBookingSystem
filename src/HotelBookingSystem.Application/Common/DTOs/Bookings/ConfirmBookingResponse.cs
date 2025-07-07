namespace HotelBookingSystem.Application.Common.DTOs.Bookings;

public class ConfirmBookingResponse
{
    public Guid Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime ConfirmedAt { get; set; }
}