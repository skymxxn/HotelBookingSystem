using HotelBookingSystem.Application.Common.DTOs;
using MediatR;

namespace HotelBookingSystem.Application.Hotels.Queries.GetHotels;

public record GetHotelsQuery : IRequest<List<HotelResponse>>;