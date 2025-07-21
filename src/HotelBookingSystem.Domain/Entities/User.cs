namespace HotelBookingSystem.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsEmailConfirmed { get; set; } = false;
    public string PasswordHash { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    
    public ICollection<Role> Roles { get; set; } = new List<Role>();
    
    public ICollection<Room> Rooms { get; set; } = new List<Room>();
    
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    
}