using FluentValidation;

namespace HotelBookingSystem.Application.Features.Bookings.Commands.CreateBooking;

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
        
        RuleFor(x => x)
            .Must(x => (x.ToDate - x.FromDate).TotalDays >= 1)
            .WithMessage("Booking duration must be at least 1 night.");

        RuleFor(x => x)
            .Must(x => (x.ToDate - x.FromDate).TotalDays <= 30)
            .WithMessage("Booking duration must not exceed 30 nights.");
    }
}