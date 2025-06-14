using FluentValidation;

namespace HotelBookingSystem.Application.Hotels.Queries.GetHotelById;

public class GetHotelByIdValidator : AbstractValidator<GetHotelByIdQuery>
{
    public GetHotelByIdValidator()
    {
        RuleFor(x => x.HotelId)
            .NotEmpty().WithMessage("Hotel ID must not be empty.");
    }
}