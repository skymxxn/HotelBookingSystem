using FluentResults;
using MediatR;

namespace HotelBookingSystem.Application.Features.Profile.Hotels.Commands.PublishHotel;

public record PublishHotelCommand(Guid HotelId) : IRequest<Result>;