namespace HotelBookingSystem.Application.Common.DTOs.Rooms;

public class RoomRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? PricePerNight { get; set; }
    public int? Capacity { get; set; }
}