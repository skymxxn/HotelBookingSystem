using FluentResults;
using HotelBookingSystem.Application.Common.DTOs;
using MediatR;

namespace HotelBookingSystem.Application.Features.Public.Hotels.Queries.GetHotel;

public record GetHotelQuery(Guid HotelId) : IRequest<Result<HotelResponse>>;