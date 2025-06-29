using FluentResults;
using HotelBookingSystem.Application.Common.DTOs;
using MediatR;

namespace HotelBookingSystem.Application.Features.Profile.Hotels.Queries.GetMyHotel;

public record GetMyHotelQuery(Guid HotelId) : IRequest<Result<HotelResponse>>;