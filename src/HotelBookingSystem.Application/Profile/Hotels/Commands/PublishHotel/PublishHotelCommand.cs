using FluentResults;
using MediatR;

namespace HotelBookingSystem.Application.Profile.Commands.PublishHotel;

public record PublishHotelCommand(Guid HotelId) : IRequest<Result>;