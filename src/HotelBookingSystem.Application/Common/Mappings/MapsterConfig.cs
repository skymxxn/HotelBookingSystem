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
    }
}