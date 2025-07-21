using FluentValidation;

namespace HotelBookingSystem.Application.Features.Rooms.Queries.GetRoom;

public class GetRoomValidator : AbstractValidator<GetRoomQuery>
{
    public GetRoomValidator()
    {
        RuleFor(x => x.RoomId)
            .NotEmpty()
            .WithMessage("Room ID must not be empty.");
    }
}