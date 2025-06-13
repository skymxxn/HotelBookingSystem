using FluentValidation;

namespace HotelBookingSystem.Application.Hotels.GetMyHotels;

public class GetMyHotelsValidator : AbstractValidator<GetMyHotelsQuery>
{
    public GetMyHotelsValidator()
    {
        RuleFor(x => x.OwnerId)
            .NotEmpty().WithMessage("User ID cannot be empty.");
    }
}