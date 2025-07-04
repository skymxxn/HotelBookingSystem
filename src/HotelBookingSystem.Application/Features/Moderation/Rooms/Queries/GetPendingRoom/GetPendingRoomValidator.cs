using FluentValidation;

namespace HotelBookingSystem.Application.Features.Moderation.Rooms.Queries.GetPendingRoom;

public class GetPendingRoomValidator : AbstractValidator<GetPendingRoomQuery>
{
    public GetPendingRoomValidator()
    {
        RuleFor(x => x.HotelId)
            .NotEmpty().WithMessage("Hotel ID must not be empty.");

        RuleFor(x => x.RoomId)
            .NotEmpty().WithMessage("Room ID must not be empty.");
    }
}