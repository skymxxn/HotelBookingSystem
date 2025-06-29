using FluentValidation;

namespace HotelBookingSystem.Application.Features.Moderation.Hotels.Queries.GetPendingHotel;

public class GetPendingHotelValidator : AbstractValidator<GetPendingHotelQuery>
{
    public GetPendingHotelValidator()
    {
        RuleFor(x => x.HotelId)
            .NotEmpty().WithMessage("Hotel ID must not be empty.");
    }
}