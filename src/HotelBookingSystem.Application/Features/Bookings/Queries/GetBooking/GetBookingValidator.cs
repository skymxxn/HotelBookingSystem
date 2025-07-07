using FluentValidation;

namespace HotelBookingSystem.Application.Features.Bookings.Queries.GetBooking;

public class GetBookingValidator : AbstractValidator<GetBookingQuery>
{
    public GetBookingValidator()
    {
        RuleFor(x => x.BookingId)
            .NotEmpty().WithMessage("Booking ID is required.")
            .NotEqual(Guid.Empty).WithMessage("Booking ID cannot be an empty GUID.");
    }
}