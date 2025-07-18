﻿using FluentValidation;

namespace HotelBookingSystem.Application.Features.Manager.Hotels.Commands.UpdateHotel;

public class UpdateHotelValidator : AbstractValidator<UpdateHotelCommand>
{
    public UpdateHotelValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(100).WithMessage("Hotel name must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Hotel description must not exceed 500 characters.");

        RuleFor(x => x.Address)
            .MaximumLength(200).WithMessage("Hotel address must not exceed 200 characters.");
    }
}