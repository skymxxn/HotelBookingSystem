using FluentValidation;

namespace HotelBookingSystem.Application.Hotels.DeleteHotel;

public class DeleteHotelValidator : AbstractValidator<DeleteHotelCommand>
{
    public DeleteHotelValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Hotel ID is required.");
    }
}