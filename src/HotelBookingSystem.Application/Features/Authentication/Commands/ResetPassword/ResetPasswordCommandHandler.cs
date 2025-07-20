using FluentResults;
using HotelBookingSystem.Application.Common.Interfaces.Authentication;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Application.Features.Authentication.Commands.ResetPassword;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result>
{
    private readonly IHotelBookingDbContext _context;
    private readonly IJwtTokenValidator _jwtTokenValidator;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<ResetPasswordCommandHandler> _logger;

    public ResetPasswordCommandHandler(IHotelBookingDbContext context, IJwtTokenValidator jwtTokenValidator, IPasswordHasher passwordHasher, ILogger<ResetPasswordCommandHandler> logger)
    {
        _context = context;
        _jwtTokenValidator = jwtTokenValidator;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var userId = _jwtTokenValidator.ValidatePasswordResetToken(request.Token);

        if (userId == null)
        {
            _logger.LogError("Invalid token for password reset: {Token}",  request.Token);
            return Result.Fail("Invalid or expired token");
        }
        
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user == null)
        {
            _logger.LogError("User not found: {UserId}", userId);
            return Result.Fail("User not found");
        }
        
        user.PasswordHash = _passwordHasher.Hash(request.NewPassword);
        await _context.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Password reset for {UserId}", userId);
        
        return Result.Ok();
    }
}