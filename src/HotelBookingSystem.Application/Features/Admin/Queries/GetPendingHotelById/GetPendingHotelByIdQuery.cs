using HotelBookingSystem.Application.Common.DTOs;
using MediatR;

namespace HotelBookingSystem.Application.Features.Admin.Queries.GetPendingHotelById;

public record GetPendingHotelByIdQuery(Guid HotelId) : IRequest<HotelResponse?>;