using FluentResults;
using HotelBookingSystem.Application.Common.DTOs;
using HotelBookingSystem.Application.Common.DTOs.Hotels;
using MediatR;

namespace HotelBookingSystem.Application.Features.Public.Hotels.Queries.GetHotels;

public record GetHotelsQuery : IRequest<Result<List<HotelResponse>>>;