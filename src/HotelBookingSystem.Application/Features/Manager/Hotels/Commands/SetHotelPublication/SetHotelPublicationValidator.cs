using FluentValidation;

namespace HotelBookingSystem.Application.Features.Manager.Hotels.Commands.SetHotelPublication;

public class SetHotelPublicationValidator : AbstractValidator<SetHotelPublicationCommand>
{
    public SetHotelPublicationValidator()
    {
        RuleFor(x => x.HotelId)
            .NotEmpty().WithMessage("Hotel ID must not be empty.");

        RuleFor(x => x.IsPublished)
            .NotNull().WithMessage("Visibility status must not be null.");
    }
}