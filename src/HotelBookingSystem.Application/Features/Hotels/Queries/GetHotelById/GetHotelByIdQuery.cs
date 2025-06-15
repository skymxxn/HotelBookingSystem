using HotelBookingSystem.Application.Common.DTOs;
using MediatR;

namespace HotelBookingSystem.Application.Features.Hotels.Queries.GetHotelById;

public record GetHotelByIdQuery(Guid HotelId) : IRequest<HotelResponse?>;