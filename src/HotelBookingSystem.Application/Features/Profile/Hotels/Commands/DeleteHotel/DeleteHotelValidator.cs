using FluentValidation;

namespace HotelBookingSystem.Application.Features.Profile.Hotels.Commands.DeleteHotel;

public class DeleteHotelValidator : AbstractValidator<DeleteHotelCommand>
{
    public DeleteHotelValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Hotel ID is required.");
    }
}