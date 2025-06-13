using FluentValidation;
using HotelBookingSystem.Application.Common;

namespace HotelBookingSystem.Application.Hotels.UpdateHotel;

public class UpdateHotelValidator : AbstractValidator<UpdateHotelCommand>
{
    public UpdateHotelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Hotel name is required.")
            .MaximumLength(100).WithMessage("Hotel name must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Hotel description must not exceed 500 characters.");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Hotel address is required.")
            .MaximumLength(200).WithMessage("Hotel address must not exceed 200 characters.");
    }
}