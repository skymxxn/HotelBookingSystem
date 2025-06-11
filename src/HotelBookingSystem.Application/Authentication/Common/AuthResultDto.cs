namespace HotelBookingSystem.Application.Authentication.Common;

public class AuthResultDto
{
    public Guid UserId { get; init; }
    public required string Email { get; init; }
    public string Token { get; init; } = string.Empty;
}