using FluentResults;
using MediatR;

namespace HotelBookingSystem.Application.Features.Moderation.Users.Commands.RemoveRoleFromUser;

public record RemoveRoleFromUserCommand(Guid UserId, Guid RoleId) : IRequest<Result>;