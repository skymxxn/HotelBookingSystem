using FluentValidation;

namespace HotelBookingSystem.Application.Admin.Queries.GetPendingHotelById;

public class GetPendingHotelByIdValidator : AbstractValidator<GetPendingHotelByIdQuery>
{
    public GetPendingHotelByIdValidator()
    {
        RuleFor(x => x.HotelId)
            .NotEmpty().WithMessage("Hotel ID must not be empty.");
    }
}