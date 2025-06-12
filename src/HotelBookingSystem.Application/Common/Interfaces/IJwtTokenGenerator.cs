using HotelBookingSystem.Domain.Entities;

namespace HotelBookingSystem.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(Guid userId, string email, IEnumerable<string> roles);
}