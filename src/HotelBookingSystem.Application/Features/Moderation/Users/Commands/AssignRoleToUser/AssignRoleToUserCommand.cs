using FluentResults;
using MediatR;

namespace HotelBookingSystem.Application.Features.Moderation.Users.Commands.AssignRoleToUser;

public record AssignRoleToUserCommand(Guid UserId, Guid RoleId) : IRequest<Result>;