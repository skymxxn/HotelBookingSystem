using HotelBookingSystem.Domain.Entities;

namespace HotelBookingSystem.Application.Common.Interfaces.Authentication;

public interface IRefreshTokenCleaner
{
    void CleanUp(User user);
}