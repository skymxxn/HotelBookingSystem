using HotelBookingSystem.Application.Common.DTOs;
using MediatR;

namespace HotelBookingSystem.Application.Features.Admin.Queries.GetPendingHotels;

public record GetPendingHotelsQuery : IRequest<List<HotelResponse>>;