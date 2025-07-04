using FluentValidation;

namespace HotelBookingSystem.Application.Features.Manager.Rooms.Commands.CreateRoom;

public class CreateRoomValidator : AbstractValidator<CreateRoomCommand>
{
    public CreateRoomValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Room name is required.")
            .MaximumLength(100).WithMessage("Room name must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1500).WithMessage("Room description must not exceed 1500 characters.");

        RuleFor(x => x.Capacity)
            .GreaterThan(0).WithMessage("Room capacity must be greater than zero.");

        RuleFor(x => x.PricePerNight)
            .GreaterThan(0).WithMessage("Price per night must be greater than $0.")
            .LessThan(10000).WithMessage("Price per night must be less than $10000.");
    }
}