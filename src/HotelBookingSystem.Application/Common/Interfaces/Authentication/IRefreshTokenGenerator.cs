using HotelBookingSystem.Domain.Entities;

namespace HotelBookingSystem.Application.Common.Interfaces.Authentication;

public interface IRefreshTokenGenerator
{
    RefreshToken GenerateToken(Guid userId);
}