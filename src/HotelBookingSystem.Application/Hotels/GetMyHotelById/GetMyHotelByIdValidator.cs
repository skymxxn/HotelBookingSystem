using FluentValidation;

namespace HotelBookingSystem.Application.Hotels.GetMyHotelById;

public class GetMyHotelByIdValidator : AbstractValidator<GetMyHotelByIdQuery>
{
    public GetMyHotelByIdValidator()
    {
        RuleFor(x => x.HotelId)
            .NotEmpty().WithMessage("Hotel ID must not be empty.");

        RuleFor(x => x.OwnerId)
            .NotEmpty().WithMessage("Owner ID must not be empty.");
    }
}