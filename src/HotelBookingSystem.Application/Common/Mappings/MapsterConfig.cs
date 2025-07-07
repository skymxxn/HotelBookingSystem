using HotelBookingSystem.Application.Common.DTOs.Bookings;
using HotelBookingSystem.Application.Common.DTOs.Rooms;
using HotelBookingSystem.Application.Common.DTOs.Users;
using HotelBookingSystem.Application.Features.Moderation.Users.DTOs;
using HotelBookingSystem.Domain.Entities;
using Mapster;

namespace HotelBookingSystem.Application.Common.Mappings;

public static class MapsterConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<User, UserResponse>
            .NewConfig()
            .Map(dest => dest.Roles,
                src => src.Roles.Select(r => r.Name).ToList());
        
        TypeAdapterConfig<Booking, BookingResponse>.NewConfig()
            .Map(dest => dest.UserDetails, src => src.User)
            .Map(dest => dest.RoomDetails, src => src.Room);
        
        TypeAdapterConfig<User, UserDetailsDto>.NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.FirstName, src => src.FirstName)
            .Map(dest => dest.LastName, src => src.LastName)
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.PhoneNumber, src => src.PhoneNumber)
            .Map(dest => dest.Roles, src => src.Roles.Select(r => r.Name).ToList());
        
        TypeAdapterConfig<Room, RoomsDetailsDto>.NewConfig()
            .Map(dest => dest.RoomId, src => src.Id)
            .Map(dest => dest.RoomName, src => src.Name)
            .Map(dest => dest.RoomDescription, src => src.Description)
            .Map(dest => dest.HotelName, src => src.Hotel.Name)
            .Map(dest => dest.HotelDescription, src => src.Hotel.Description)
            .Map(dest => dest.HotelAddress, src => src.Hotel.Address);
    }
}