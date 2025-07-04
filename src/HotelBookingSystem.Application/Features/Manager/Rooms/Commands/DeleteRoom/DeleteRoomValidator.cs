using FluentValidation;

namespace HotelBookingSystem.Application.Features.Manager.Rooms.Commands.DeleteRoom;

public class DeleteRoomValidator : AbstractValidator<DeleteRoomCommand>
{
    public DeleteRoomValidator()
    {
        RuleFor(x => x.HotelId)
            .NotEmpty().WithMessage("Hotel ID must not be empty.");

        RuleFor(x => x.RoomId)
            .NotEmpty().WithMessage("Room ID must not be empty.");
    }
}