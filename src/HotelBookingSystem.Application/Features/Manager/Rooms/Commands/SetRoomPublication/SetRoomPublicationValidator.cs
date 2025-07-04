using FluentValidation;

namespace HotelBookingSystem.Application.Features.Manager.Rooms.Commands.SetRoomPublication;

public class SetRoomPublicationValidator : AbstractValidator<SetRoomPublicationCommand>
{
    public SetRoomPublicationValidator()
    {
        RuleFor(x => x.RoomId)
            .NotEmpty().WithMessage("Room ID must not be empty.");

        RuleFor(x => x.IsPublished)
            .NotNull().WithMessage("Publication status must not be null.");
    }
}