using HotelBookingSystem.Application.Common.DTOs;
using MediatR;

namespace HotelBookingSystem.Application.Admin.Queries.GetPendingHotelById;

public record GetPendingHotelByIdQuery(Guid HotelId) : IRequest<HotelResponse?>;