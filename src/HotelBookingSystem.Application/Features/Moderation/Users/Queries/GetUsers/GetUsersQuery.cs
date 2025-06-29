using FluentResults;
using HotelBookingSystem.Application.Features.Moderation.Users.DTOs;
using MediatR;

namespace HotelBookingSystem.Application.Features.Moderation.Users.Queries.GetUsers;

public record GetUsersQuery : IRequest<Result<List<UserResponse>>>;