using HotelBookingSystem.Application.Common.DTOs;
using MediatR;

namespace HotelBookingSystem.Application.Features.Profile.Hotels.Queries.GetMyHotels;

public record GetMyHotelsQuery() : IRequest<List<HotelResponse>>;