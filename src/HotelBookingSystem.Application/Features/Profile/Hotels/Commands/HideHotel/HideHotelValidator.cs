using FluentValidation;

namespace HotelBookingSystem.Application.Features.Profile.Hotels.Commands.HideHotel;

public class HideHotelValidator : AbstractValidator<HideHotelCommand>
{
    public HideHotelValidator()
    {
        RuleFor(x => x.HotelId)
            .NotEmpty().WithMessage("Hotel ID must not be empty.");
    }
}