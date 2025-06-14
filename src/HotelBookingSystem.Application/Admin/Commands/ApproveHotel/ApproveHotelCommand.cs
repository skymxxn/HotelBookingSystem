using FluentResults;
using MediatR;

namespace HotelBookingSystem.Application.Admin.Commands.ApproveHotel;

public record ApproveHotelCommand(Guid HotelId) : IRequest<Result>;