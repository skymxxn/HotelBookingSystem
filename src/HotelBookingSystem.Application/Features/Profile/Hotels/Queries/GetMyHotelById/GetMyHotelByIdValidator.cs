using FluentValidation;

namespace HotelBookingSystem.Application.Features.Profile.Hotels.Queries.GetMyHotelById;

public class GetMyHotelByIdValidator : AbstractValidator<GetMyHotelByIdQuery>
{
    public GetMyHotelByIdValidator()
    {
        RuleFor(x => x.HotelId)
            .NotEmpty().WithMessage("Hotel ID must not be empty.");
    }
}