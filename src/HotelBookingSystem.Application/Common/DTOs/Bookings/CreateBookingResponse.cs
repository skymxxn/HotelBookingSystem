namespace HotelBookingSystem.Application.Common.DTOs.Bookings;

public class CreateBookingResponse
{
    public Guid Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public decimal TotalPrice { get; set; }
}