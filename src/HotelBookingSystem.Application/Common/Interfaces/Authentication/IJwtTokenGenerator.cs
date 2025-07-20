namespace HotelBookingSystem.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateAccessToken(Guid userId, string email, IEnumerable<string> roles);
    string GenerateEmailVerificationToken(Guid userId);
}