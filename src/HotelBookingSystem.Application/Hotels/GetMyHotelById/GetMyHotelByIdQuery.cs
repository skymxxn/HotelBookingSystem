using HotelBookingSystem.Application.Common.DTOs;
using MediatR;

namespace HotelBookingSystem.Application.Hotels.GetMyHotelById;

public record GetMyHotelByIdQuery(Guid OwnerId, Guid HotelId) : IRequest<HotelResponse?>;