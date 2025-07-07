using FluentValidation;

namespace HotelBookingSystem.Application.Features.Manager.Bookings.Queries.GetBooking;

public class GetBookingValidator : AbstractValidator<GetBookingQuery>
{
    public GetBookingValidator()
    {
        RuleFor(x => x.BookingId)
            .NotEmpty().WithMessage("Booking ID must not be empty.");
    }
}