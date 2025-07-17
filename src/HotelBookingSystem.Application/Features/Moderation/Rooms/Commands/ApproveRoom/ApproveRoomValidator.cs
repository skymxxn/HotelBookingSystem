using FluentValidation;

namespace HotelBookingSystem.Application.Features.Moderation.Rooms.Commands.ApproveRoom;

public class ApproveRoomValidator : AbstractValidator<ApproveRoomCommand>
{
    public ApproveRoomValidator()
    {
        RuleFor(x => x.RoomId)
            .NotEmpty().WithMessage("Room ID must not be empty.");
    }
}