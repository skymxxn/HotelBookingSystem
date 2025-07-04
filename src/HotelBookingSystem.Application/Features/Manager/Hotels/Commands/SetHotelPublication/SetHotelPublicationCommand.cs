using FluentResults;
using MediatR;

namespace HotelBookingSystem.Application.Features.Manager.Hotels.Commands.SetHotelPublication;

public record SetHotelPublicationCommand(Guid HotelId, bool IsPublished) : IRequest<Result>;