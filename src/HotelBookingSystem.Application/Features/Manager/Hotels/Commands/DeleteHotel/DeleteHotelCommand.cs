using FluentResults;
using MediatR;

namespace HotelBookingSystem.Application.Features.Manager.Hotels.Commands.DeleteHotel;

public record DeleteHotelCommand(Guid Id) : IRequest<Result>;