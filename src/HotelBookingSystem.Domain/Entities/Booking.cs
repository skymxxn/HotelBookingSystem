namespace HotelBookingSystem.Domain.Entities;

public class Booking
{
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public decimal TotalPrice { get; set; }
    
    public Guid RoomId { get; set; }
    public Room Room { get; set; } = null!;
    
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}