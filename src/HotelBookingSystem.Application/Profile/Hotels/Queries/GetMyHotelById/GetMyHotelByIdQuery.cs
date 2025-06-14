using HotelBookingSystem.Application.Common.DTOs;
using MediatR;

namespace HotelBookingSystem.Application.Profile.Queries.GetMyHotelById;

public record GetMyHotelByIdQuery(Guid HotelId) : IRequest<HotelResponse?>;