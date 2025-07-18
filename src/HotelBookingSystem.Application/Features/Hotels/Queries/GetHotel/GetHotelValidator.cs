﻿using FluentValidation;

namespace HotelBookingSystem.Application.Features.Hotels.Queries.GetHotel;

public class GetHotelValidator : AbstractValidator<GetHotelQuery>
{
    public GetHotelValidator()
    {
        RuleFor(x => x.HotelId)
            .NotEmpty().WithMessage("Hotel ID must not be empty.");
    }
}