using FluentValidation;

namespace HotelBookingSystem.Application.Features.Manager.Rooms.Commands.UpdateRoom;

public class UpdateRoomValidator : AbstractValidator<UpdateRoomCommand>
{
    public UpdateRoomValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Room name must not be empty.")
            .MaximumLength(100).WithMessage("Room name must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Room description must not exceed 1500 characters.");

        RuleFor(x => x.Capacity)
            .GreaterThan(0).WithMessage("Room capacity must be greater than zero.");

        RuleFor(x => x.PricePerNight)
            .GreaterThan(0).WithMessage("Price per night must be greater than $0.")
            .LessThan(10000).WithMessage("Price per night must be less than $10000.");
    }
}