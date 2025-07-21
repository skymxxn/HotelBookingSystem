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
    Guid? HotelId,
    
    string? SortBy = null,
    string SortOrder = "desc",
    
    int Page = 1,
    int PageSize = 10
) : IRequest<Result<List<RoomResponse>>>;