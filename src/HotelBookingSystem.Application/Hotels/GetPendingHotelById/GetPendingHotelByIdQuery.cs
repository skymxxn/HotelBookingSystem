using HotelBookingSystem.Application.Common.DTOs;
using MediatR;

namespace HotelBookingSystem.Application.Hotels.GetPendingHotelById;

public record GetPendingHotelByIdQuery(Guid HotelId) : IRequest<HotelResponse?>;