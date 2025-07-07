namespace HotelBookingSystem.Application.Common.DTOs.Rooms;

public class RoomsDetailsDto
{
    public Guid RoomId { get; set; }
    public string RoomName { get; set; } = string.Empty;
    public string RoomDescription { get; set; } = string.Empty;
    public decimal PricePerNight { get; set; }
    public int Capacity { get; set; }
    public string HotelName { get; set; } = string.Empty;
    public string HotelDescription { get; set; } = string.Empty;
    public string HotelAddress { get; set; } = string.Empty;
}