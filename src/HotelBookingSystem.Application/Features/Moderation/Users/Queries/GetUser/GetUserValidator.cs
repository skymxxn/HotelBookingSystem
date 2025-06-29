using FluentValidation;

namespace HotelBookingSystem.Application.Features.Moderation.Users.Queries.GetUser;

public class GetUserValidator : AbstractValidator<GetUserQuery>
{
    public GetUserValidator()
    {
        RuleFor(query => query.UserId)
            .NotEmpty().WithMessage("User ID must not be empty.");
    }
}