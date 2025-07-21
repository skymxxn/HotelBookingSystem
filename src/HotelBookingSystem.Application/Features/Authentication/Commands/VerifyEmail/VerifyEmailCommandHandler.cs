using FluentResults;
using HotelBookingSystem.Application.Common.Interfaces.Authentication;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Application.Features.Authentication.Commands.VerifyEmail;

public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, Result>
{
    private readonly IHotelBookingDbContext  _context;
    private readonly IJwtTokenValidator _jwtTokenValidator;
    private readonly ILogger<VerifyEmailCommandHandler> _logger;

    public VerifyEmailCommandHandler(IHotelBookingDbContext context, IJwtTokenValidator jwtTokenValidator, ILogger<VerifyEmailCommandHandler> logger)
    {
        _context = context;
        _jwtTokenValidator = jwtTokenValidator;
        _logger = logger;
    }

    public async Task<Result> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        var userId = _jwtTokenValidator.ValidateEmailVerificationToken(request.Token);
        if (userId == null)
        {
            _logger.LogError("Invalid token for email verification {UserId}", userId);
            return Result.Fail("Invalid or expired token");
        }

        var user = await _context.Users.FirstOrDefaultAsync(u =>  u.Id == userId, cancellationToken);
        if (user == null)
        {
            _logger.LogError("User with ID {userId} not found", userId);
            return Result.Fail("User not found");
        }

        if (user.IsEmailConfirmed)
        {
            _logger.LogError("User with ID {userId} is already confirmed", userId);
            return Result.Fail("Email is already confirmed");
        }
        
        user.IsEmailConfirmed = true;
        await _context.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("User with ID {userId} verified", user.Id);
        return Result.Ok();
    }
}