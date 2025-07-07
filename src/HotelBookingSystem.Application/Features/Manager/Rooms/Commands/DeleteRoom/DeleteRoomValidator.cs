using FluentValidation;

namespace HotelBookingSystem.Application.Features.Manager.Rooms.Commands.DeleteRoom;

public class DeleteRoomValidator : AbstractValidator<DeleteRoomCommand>
{
    public DeleteRoomValidator()
    {
        RuleFor(x => x.RoomId)
            .NotEmpty().WithMessage("Room ID must not be empty.");
    }
}