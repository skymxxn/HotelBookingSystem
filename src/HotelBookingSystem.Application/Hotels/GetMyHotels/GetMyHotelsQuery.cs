using HotelBookingSystem.Application.Common.DTOs;
using MediatR;

namespace HotelBookingSystem.Application.Hotels.GetMyHotels;

public record GetMyHotelsQuery(Guid OwnerId) : IRequest<List<HotelResponse>>;