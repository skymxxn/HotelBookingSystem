using FluentResults;
using MediatR;

namespace HotelBookingSystem.Application.Features.Admin.Commands.ApproveHotel;

public record ApproveHotelCommand(Guid HotelId) : IRequest<Result>;