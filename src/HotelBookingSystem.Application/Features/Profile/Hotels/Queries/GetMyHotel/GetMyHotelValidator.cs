using FluentValidation;

namespace HotelBookingSystem.Application.Features.Profile.Hotels.Queries.GetMyHotel;

public class GetMyHotelValidator : AbstractValidator<GetMyHotelQuery>
{
    public GetMyHotelValidator()
    {
        RuleFor(x => x.HotelId)
            .NotEmpty().WithMessage("Hotel ID must not be empty.");
    }
}