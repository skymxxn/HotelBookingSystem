using FluentResults;
using MediatR;

namespace HotelBookingSystem.Application.Features.Profile.Hotels.Commands.DeleteHotel;

public record DeleteHotelCommand(Guid Id) : IRequest<Result>;