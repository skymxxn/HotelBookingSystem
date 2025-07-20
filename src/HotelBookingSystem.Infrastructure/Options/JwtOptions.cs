namespace HotelBookingSystem.Infrastructure.Options;

public class JwtOptions
{
    public JwtTokenTypeOptions AccessToken { get; set; } = null!;
    public JwtTokenTypeOptions EmailVerification { get; set; } = null!;
    public JwtTokenTypeOptions PasswordReset { get; set; } = null!;
    public int RefreshTokenLifeTimeDays { get; set; }
}