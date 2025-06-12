using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities;

namespace HotelBookingSystem.Infrastructure.Authentication;

public class RefreshTokenCleaner : IRefreshTokenCleaner
{
    private readonly IHotelBookingDbContext _context;
    
    public RefreshTokenCleaner(IHotelBookingDbContext context)
    {
        _context = context;
    }
    
    public void CleanUp(User user)
    {
        if (user.RefreshTokens.Count > 5)
        {
            var oldestToken = user.RefreshTokens.OrderBy(rt => rt.CreatedAt).First();
            _context.RefreshTokens.Remove(oldestToken);
        }
    }
}