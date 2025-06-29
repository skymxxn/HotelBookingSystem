using FluentValidation;

namespace HotelBookingSystem.Application.Features.Moderation.Users.Commands.AssignRoleToUser;

public class AssignRoleToUserValidator : AbstractValidator<AssignRoleToUserCommand>
{
    public AssignRoleToUserValidator()
    {
        RuleFor(command => command.UserId)
            .NotEmpty().WithMessage("User ID must not be empty.");

        RuleFor(command => command.RoleId)
            .NotEmpty().WithMessage("Role ID must not be empty.");
    }
}