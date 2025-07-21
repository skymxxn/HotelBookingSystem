using HotelBookingSystem.Domain.Entities;

namespace HotelBookingSystem.Application.Common.Interfaces.Access;

public interface IAccessService
{
    IQueryable<Booking> ApplyBookingAccessFilter(IQueryable<Booking> query);
    IQueryable<Hotel> ApplyHotelAccessFilter(IQueryable<Hotel> query);
    IQueryable<Room> ApplyRoomAccessFilter(IQueryable<Room> query);
}