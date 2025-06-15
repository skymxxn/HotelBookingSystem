using HotelBookingSystem.Application.Common.DTOs;
using MediatR;

namespace HotelBookingSystem.Application.Features.Hotels.Queries.GetHotels;

public record GetHotelsQuery : IRequest<List<HotelResponse>>;