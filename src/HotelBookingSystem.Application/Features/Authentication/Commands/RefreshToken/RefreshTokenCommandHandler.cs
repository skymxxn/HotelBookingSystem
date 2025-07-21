using FluentResults;
using FluentValidation;
using HotelBookingSystem.Application.Common.Interfaces.Authentication;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using HotelBookingSystem.Application.Features.Authentication.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Application.Features.Authentication.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<AuthResultDto>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IHotelBookingDbContext _context;
    private readonly IRefreshTokenCleaner _refreshTokenCleaner;
    private readonly IValidator<RefreshTokenCommand> _validator;
    private readonly ILogger<RefreshTokenCommandHandler> _logger;
    
    public RefreshTokenCommandHandler(IJwtTokenGenerator jwtTokenGenerator, IHotelBookingDbContext context, IRefreshTokenGenerator refreshTokenGenerator, IRefreshTokenCleaner refreshTokenCleaner, IValidator<RefreshTokenCommand> validator, ILogger<RefreshTokenCommandHandler> logger)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _context = context;
        _refreshTokenGenerator = refreshTokenGenerator;
        _refreshTokenCleaner = refreshTokenCleaner;
        _validator = validator;
        _logger = logger;
    }
    
    public async Task<Result<AuthResultDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for RefreshTokenCommand: {Errors}", validationResult.Errors);
            return Result.Fail(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
        }
        
        var refreshToken = await _context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken, cancellationToken);

        if (refreshToken == null || refreshToken.IsRevoked)
        {
            _logger.LogWarning("Refresh token {RefreshToken} is invalid or revoked", request.RefreshToken);
            return await Task.FromResult(Result.Fail<AuthResultDto>("Invalid refresh token."));
        }
        
        if (refreshToken.ExpiresAt < DateTime.UtcNow)
        {
            _logger.LogWarning("Refresh token {RefreshToken} has expired", request.RefreshToken);
            return await Task.FromResult(Result.Fail<AuthResultDto>("Refresh token has expired."));
        }
        
        var user = await _context.Users
            .Include(u => u.RefreshTokens)
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Id == refreshToken.UserId,cancellationToken);
        
        if (user == null)
        {
            _logger.LogWarning("Refresh token {RefreshToken} does not belong to a valid user", request.RefreshToken);
            return await Task.FromResult(Result.Fail<AuthResultDto>("Refresh token does not belong to a valid user."));
        }
        
        refreshToken.IsRevoked = true;
        
        var newRefreshToken = _refreshTokenGenerator.GenerateToken(user.Id);
        user.RefreshTokens.Add(newRefreshToken);
        
        _refreshTokenCleaner.CleanUp(user);
        
        var accessToken = _jwtTokenGenerator.GenerateAccessToken(user.Id, user.Email, user.Roles.Select(r => r.Name).ToList());
        
        await _context.SaveChangesAsync(cancellationToken);
        
        var authResult = new AuthResultDto
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken.Token,
        };
        
        _logger.LogInformation("User {UserId} refreshed tokens successfully", user.Id);
        
        return Result.Ok(authResult);
    }
}