using FluentValidation;

namespace HotelBookingSystem.Application.Features.Manager.Rooms.Queries.GetRoom;

public class GetRoomValidator : AbstractValidator<GetRoomQuery>
{
    public GetRoomValidator()
    {
        RuleFor(x => x.HotelId)
            .NotEmpty().WithMessage("Hotel ID must not be empty.");

        RuleFor(x => x.RoomId)
            .NotEmpty().WithMessage("Room ID must not be empty.");
    }
}