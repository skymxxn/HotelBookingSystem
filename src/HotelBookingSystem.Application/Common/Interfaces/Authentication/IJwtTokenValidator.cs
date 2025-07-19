namespace HotelBookingSystem.Application.Common.Interfaces.Authentication;

public interface IJwtTokenValidator
{
    Guid? ValidateEmailVerificationToken(string token);
}