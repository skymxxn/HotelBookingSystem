using FluentResults;
using HotelBookingSystem.Application.Features.Moderation.Users.DTOs;
using MediatR;

namespace HotelBookingSystem.Application.Features.Moderation.Users.Queries.GetUser;

public record GetUserQuery(Guid UserId) : IRequest<Result<UserResponse>>;