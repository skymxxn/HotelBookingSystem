using FluentResults;
using MediatR;

namespace HotelBookingSystem.Application.Profile.Commands.DeleteHotel;

public record DeleteHotelCommand(Guid Id) : IRequest<Result>;