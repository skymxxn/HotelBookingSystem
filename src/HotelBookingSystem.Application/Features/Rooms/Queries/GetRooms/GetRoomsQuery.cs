using FluentResults;
using HotelBookingSystem.Application.Common.DTOs.Rooms;
using MediatR;

namespace HotelBookingSystem.Application.Features.Rooms.Queries.GetRooms;

public record GetRoomsQuery(
    string? Name,
    int? Capacity,
    decimal? MinPrice,
    decimal? MaxPrice,
    bool? IsApproved,
    bool? IsPublished,
    DateTime? CreatedAt,
    DateTime? UpdatedAt,
    Guid? HotelId
) : IRequest<Result<List<RoomResponse>>>;