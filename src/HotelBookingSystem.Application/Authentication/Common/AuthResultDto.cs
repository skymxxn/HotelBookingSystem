namespace HotelBookingSystem.Application.Authentication.Common;

public class AuthResultDto
{
    public string AccessToken { get; init; } = string.Empty;
    public string RefreshToken { get; init; } = string.Empty;
}