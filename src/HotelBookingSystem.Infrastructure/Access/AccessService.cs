using HotelBookingSystem.Application.Common.Interfaces.Access;
using HotelBookingSystem.Application.Common.Interfaces.Users;
using HotelBookingSystem.Domain.Entities;

namespace HotelBookingSystem.Infrastructure.Access;

public class AccessService : IAccessService
{
    private readonly ICurrentUserService _currentUser;

    public AccessService(ICurrentUserService currentUser)
    {
        _currentUser = currentUser;
    }

    public IQueryable<Booking> ApplyBookingAccessFilter(IQueryable<Booking> query)
    {
        var userId = _currentUser.GetUserId();
        
        if (_currentUser.IsManager())
        {
            query = query.Where(b => b.Room.Hotel.OwnerId == userId);
        }
        else if (!_currentUser.IsAdmin() && !_currentUser.IsModerator())
        {
            query = query.Where(b => b.UserId == userId);
        }
        
        return query;
    }

    public IQueryable<Hotel> ApplyHotelAccessFilter(IQueryable<Hotel> query)
    {
        var userId = _currentUser.GetUserId();
        
        if (_currentUser.IsUser())
        {
            query = query.Where(h => h.IsPublished && h.IsApproved);
        }
        else if (_currentUser.IsManager())
        {
            query = query.Where(h => h.OwnerId == userId);
        }
        
        return query;
    }

    public IQueryable<Room> ApplyRoomAccessFilter(IQueryable<Room> query)
    {
        var userId = _currentUser.GetUserId();
        
        if (_currentUser.IsUser())
        {
            query = query.Where(r => r.IsPublished && r.IsApproved);
        }
        else if (_currentUser.IsManager())
        {
            query = query.Where(r => r.Hotel.OwnerId == userId);
        }
        
        return query;
    }
}