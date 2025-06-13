using HotelBookingSystem.Application.Common.DTOs;
using MediatR;

namespace HotelBookingSystem.Application.Hotels.GetPendingHotels;

public record GetPendingHotelsQuery : IRequest<List<HotelResponse>>;