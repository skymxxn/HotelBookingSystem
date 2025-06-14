using FluentValidation;

namespace HotelBookingSystem.Application.Profile.Commands.PublishHotel;

public class PublishHotelValidator : AbstractValidator<PublishHotelCommand>
{
    public PublishHotelValidator()
    {
        RuleFor(command => command.HotelId)
            .NotEmpty().WithMessage("Hotel ID must not be empty.");
    }
}