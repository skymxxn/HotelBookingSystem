using HotelBookingSystem.Domain.Entities;

namespace HotelBookingSystem.Application.Common.Interfaces;

public interface IRefreshTokenGenerator
{
    RefreshToken GenerateToken(Guid userId);
}