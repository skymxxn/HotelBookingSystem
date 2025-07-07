using FluentValidation;

namespace HotelBookingSystem.Application.Features.Manager.Bookings.Commands.ConfirmBooking;

public class ConfirmBookingValidator : AbstractValidator<ConfirmBookingCommand>
{
    public ConfirmBookingValidator()
    {
        RuleFor(x => x.BookingId)
            .NotEmpty().WithMessage("Booking ID must not be empty.")
            .NotNull().WithMessage("Booking ID must not be null.");
    }
}