namespace HotelBookingSystem.Infrastructure.Options;

public class JwtTokenTypeOptions
{
    public string Key { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public int LifetimeMinutes { get; set; }
}