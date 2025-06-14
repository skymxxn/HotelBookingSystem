using FluentResults;
using MediatR;

namespace HotelBookingSystem.Application.Profile.Commands.HideHotel;

public record HideHotelCommand(Guid HotelId) : IRequest<Result>;