using FluentValidation;

namespace HotelBookingSystem.Application.Features.Authentication.Commands.Logout;

public class LogoutValidator : AbstractValidator<LogoutCommand>
{
    public LogoutValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("Refresh token is required.");
    }
}