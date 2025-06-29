using FluentValidation;

namespace HotelBookingSystem.Application.Features.Moderation.Users.Commands.RemoveRoleFromUser;

public class RemoveRoleFromUserValidator : AbstractValidator<RemoveRoleFromUserCommand>
{
    public RemoveRoleFromUserValidator()
    {
        RuleFor(command => command.UserId)
            .NotEmpty().WithMessage("User ID must not be empty.");

        RuleFor(command => command.RoleId)
            .NotEmpty().WithMessage("Role ID must not be empty.");
    }
}