using FluentResults;
using HotelBookingSystem.Application.Common.DTOs;
using HotelBookingSystem.Application.Common.DTOs.Rooms;
using MediatR;

namespace HotelBookingSystem.Application.Features.Manager.Rooms.Commands.SetRoomPublication;

public record SetRoomPublicationCommand(Guid HotelId, Guid RoomId, bool IsPublished) : IRequest<Result<RoomResponse>>;