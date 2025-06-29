using FluentResults;
using HotelBookingSystem.Application.Common.DTOs;
using MediatR;

namespace HotelBookingSystem.Application.Features.Moderation.Hotels.Queries.GetPendingHotel;

public record GetPendingHotelQuery(Guid HotelId) : IRequest<Result<HotelResponse>>;