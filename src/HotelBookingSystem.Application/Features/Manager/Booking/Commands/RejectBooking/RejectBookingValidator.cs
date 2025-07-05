using FluentValidation;

namespace HotelBookingSystem.Application.Features.Manager.Booking.Commands.RejectBooking;

public class RejectBookingValidator : AbstractValidator<RejectBookingCommand>
{
    public RejectBookingValidator()
    {
        RuleFor(x => x.BookingId)
            .NotEmpty().WithMessage("Booking ID must not be empty.");

        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("Reason for rejection must not be empty.");
    }
}