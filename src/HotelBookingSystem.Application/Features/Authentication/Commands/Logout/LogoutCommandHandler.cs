using FluentResults;
using FluentValidation;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using HotelBookingSystem.Application.Common.Interfaces.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Application.Features.Authentication.Commands.Logout;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result>
{
    private readonly IHotelBookingDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<LogoutCommand> _validator;
    private readonly ILogger<LogoutCommandHandler> _logger;
    
    public LogoutCommandHandler(IHotelBookingDbContext context, ILogger<LogoutCommandHandler> logger, IValidator<LogoutCommand> validator, ICurrentUserService currentUser)
    {
        _context = context;
        _logger = logger;
        _validator = validator;
        _currentUser = currentUser;
    }
    
    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for LogoutCommand: {Errors}", validationResult.Errors);
            return Result.Fail(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
        }
        
        var refreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(t => t.Token == request.RefreshToken, cancellationToken);
        
        if (refreshToken == null || refreshToken.IsRevoked)
        {
            _logger.LogWarning("Refresh token not found or already revoked for user {UserId}", _currentUser.GetUserId());
            return Result.Fail(new Error("Invalid or already revoked refresh token."));
        }
        
        refreshToken.IsRevoked = true;
        await _context.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("User {UserId} logged out successfully", _currentUser.GetUserId());
        
        return Result.Ok();
    }
}