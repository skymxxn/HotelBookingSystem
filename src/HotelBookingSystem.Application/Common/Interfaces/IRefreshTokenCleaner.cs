using HotelBookingSystem.Domain.Entities;

namespace HotelBookingSystem.Application.Common.Interfaces;

public interface IRefreshTokenCleaner
{
    void CleanUp(User user);
}