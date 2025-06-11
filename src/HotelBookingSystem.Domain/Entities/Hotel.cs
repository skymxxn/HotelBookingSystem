namespace HotelBookingSystem.Domain.Entities;

public class Hotel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsApproved { get; set; }
    
    public Guid OwnerId { get; set; }
    public User Owner { get; set; } = null!;
    
    public ICollection<Room> Rooms { get; set; } = new List<Room>();
}