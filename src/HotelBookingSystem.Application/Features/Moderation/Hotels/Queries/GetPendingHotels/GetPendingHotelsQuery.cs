using FluentResults;
using HotelBookingSystem.Application.Common.DTOs;
using MediatR;

namespace HotelBookingSystem.Application.Features.Moderation.Hotels.Queries.GetPendingHotels;

public record GetPendingHotelsQuery : IRequest<Result<List<HotelResponse>>>;