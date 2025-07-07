using FluentValidation;

namespace HotelBookingSystem.Application.Features.Public.Bookings.Commands.CancelBooking;

public class CancelBookingValidator : AbstractValidator<CancelBookingCommand>
{
    public CancelBookingValidator()
    {
        RuleFor(x => x.BookingId)
            .NotEmpty().WithMessage("Booking ID is required.")
            .NotEqual(Guid.Empty).WithMessage("Booking ID cannot be an empty GUID.");

        RuleFor(x => x.Reason)
            .MaximumLength(1500).WithMessage("Reason for cancellation must not exceed 1500 characters.");
    }
}