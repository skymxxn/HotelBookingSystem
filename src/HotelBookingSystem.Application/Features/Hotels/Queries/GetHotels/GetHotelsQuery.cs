using FluentResults;
using HotelBookingSystem.Application.Common.DTOs.Hotels;
using MediatR;

namespace HotelBookingSystem.Application.Features.Hotels.Queries.GetHotels;

public record GetHotelsQuery(
    string? Name,
    string? Address,
    bool? IsApproved,
    bool? IsPublished,
    DateTime? CreatedAt,
    DateTime? UpdatedAt,
    Guid? OwnerId
    ) : IRequest<Result<List<HotelResponse>>>;