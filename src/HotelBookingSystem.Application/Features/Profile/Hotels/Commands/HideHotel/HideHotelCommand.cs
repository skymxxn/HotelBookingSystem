using FluentResults;
using MediatR;

namespace HotelBookingSystem.Application.Features.Profile.Hotels.Commands.HideHotel;

public record HideHotelCommand(Guid HotelId) : IRequest<Result>;