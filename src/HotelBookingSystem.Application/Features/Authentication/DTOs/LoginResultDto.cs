namespace HotelBookingSystem.Application.Features.Authentication.DTOs;

public class LoginResultDto
{
    public Guid UserId { get; init; }
    public required string Email { get; init; }
    public string Token { get; init; } = string.Empty;
}