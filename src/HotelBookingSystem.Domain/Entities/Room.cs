namespace HotelBookingSystem.Domain.Entities;

public class Room
{
    public Guid Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public decimal PricePerNight { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsAvailable { get; set; }
    
    public Guid HotelId { get; set; }
    public Hotel Hotel { get; set; } = null!;
    
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}