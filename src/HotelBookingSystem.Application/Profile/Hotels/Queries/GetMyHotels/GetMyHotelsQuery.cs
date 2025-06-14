using HotelBookingSystem.Application.Common.DTOs;
using MediatR;

namespace HotelBookingSystem.Application.Profile.Queries.GetMyHotels;

public record GetMyHotelsQuery() : IRequest<List<HotelResponse>>;