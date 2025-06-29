using FluentResults;
using MediatR;

namespace HotelBookingSystem.Application.Features.Moderation.Hotels.Commands.ApproveHotel;

public record ApproveHotelCommand(Guid HotelId) : IRequest<Result>;