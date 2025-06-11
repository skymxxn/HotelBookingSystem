using HotelBookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Common.Interfaces;

public interface IHotelBookingDbContext
{
    DbSet<User> Users { get; }
    DbSet<Role> Roles { get; }
    DbSet<Hotel> Hotels { get; }
    DbSet<Room> Rooms { get; }
    DbSet<Booking> Bookings { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}