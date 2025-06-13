using FluentResults;
using MediatR;

namespace HotelBookingSystem.Application.Hotels.DeleteHotel;

public record DeleteHotelCommand(Guid Id) : IRequest<Result>;