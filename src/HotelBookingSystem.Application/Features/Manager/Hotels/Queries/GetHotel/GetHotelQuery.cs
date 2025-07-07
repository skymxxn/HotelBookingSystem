using FluentResults;
using HotelBookingSystem.Application.Common.DTOs;
using HotelBookingSystem.Application.Common.DTOs.Hotels;
using MediatR;

namespace HotelBookingSystem.Application.Features.Manager.Hotels.Queries.GetHotel;

public record GetHotelQuery(Guid HotelId) : IRequest<Result<HotelResponse>>;