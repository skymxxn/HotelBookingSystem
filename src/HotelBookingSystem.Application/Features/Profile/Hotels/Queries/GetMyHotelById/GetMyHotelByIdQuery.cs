using HotelBookingSystem.Application.Common.DTOs;
using MediatR;

namespace HotelBookingSystem.Application.Features.Profile.Hotels.Queries.GetMyHotelById;

public record GetMyHotelByIdQuery(Guid HotelId) : IRequest<HotelResponse?>;