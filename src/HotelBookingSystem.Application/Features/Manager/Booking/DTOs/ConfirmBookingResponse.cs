namespace HotelBookingSystem.Application.Features.Manager.Booking.DTOs;

public class ConfirmBookingResponse
{
    public Guid BookingId { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime ConfirmationDate { get; set; }
}