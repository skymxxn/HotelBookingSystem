using FluentValidation;

namespace HotelBookingSystem.Application.Features.Public.Bookings.Commands.CreateBooking;

public class CreateBookingValidator : AbstractValidator<CreateBookingCommand>
{
    public CreateBookingValidator()
    {
        RuleFor(x => x.RoomId)
            .NotEmpty().WithMessage("Room ID is required.");

        RuleFor(x => x.FromDate)
            .NotEmpty().WithMessage("From date is required.")
            .LessThanOrEqualTo(x => x.ToDate).WithMessage("From date must be before or equal to To date.");

        RuleFor(x => x.ToDate)
            .NotEmpty().WithMessage("To date is required.")
            .GreaterThanOrEqualTo(x => x.FromDate).WithMessage("To date must be after or equal to From date.");

        RuleFor(x => x.Description)
            .MaximumLength(1500).WithMessage("Description cannot exceed 1500 characters.");
    }
}