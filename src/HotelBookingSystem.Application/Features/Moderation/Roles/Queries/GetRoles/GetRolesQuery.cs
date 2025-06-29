using FluentResults;
using HotelBookingSystem.Application.Features.Moderation.Roles.DTOs;
using MediatR;

namespace HotelBookingSystem.Application.Features.Moderation.Roles.Queries.GetRoles;

public record GetRolesQuery : IRequest<Result<List<RoleResponse>>>;