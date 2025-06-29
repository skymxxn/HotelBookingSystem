using FluentValidation;

namespace HotelBookingSystem.Application.Features.Moderation.Hotels.Commands.ApproveHotel;

public class ApproveHotelValidator : AbstractValidator<ApproveHotelCommand>
{
    public ApproveHotelValidator()
    {
        RuleFor(x => x.HotelId)
            .NotEmpty().WithMessage("Hotel ID is required.")
            .Must(BeAValidGuid).WithMessage("Hotel ID must be a valid GUID.");
    }

    private static bool BeAValidGuid(Guid hotelId)
    {
        return hotelId != Guid.Empty;
    }
}