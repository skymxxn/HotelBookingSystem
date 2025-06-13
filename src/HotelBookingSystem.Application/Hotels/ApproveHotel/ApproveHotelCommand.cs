using FluentResults;
using HotelBookingSystem.Application.Common.DTOs;
using MediatR;

namespace HotelBookingSystem.Application.Hotels.ApproveHotel;

public record ApproveHotelCommand(Guid HotelId) : IRequest<Result>;