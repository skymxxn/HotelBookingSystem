using FluentResults;
using FluentValidation;
using HotelBookingSystem.Application.Common.Interfaces.Authentication;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using HotelBookingSystem.Application.Features.Authentication.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Application.Features.Authentication.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthResultDto>>
{
    private readonly IHotelBookingDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IRefreshTokenCleaner _refreshTokenCleaner;
    private readonly IValidator<LoginCommand> _validator;
    private readonly ILogger<LoginCommandHandler> _logger;
    
    public LoginCommandHandler(IHotelBookingDbContext context, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator, IRefreshTokenGenerator refreshTokenGenerator, IRefreshTokenCleaner refreshTokenCleaner, IValidator<LoginCommand> validator, ILogger<LoginCommandHandler> logger)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
        _refreshTokenCleaner = refreshTokenCleaner;
        _validator = validator;
        _logger = logger;
    }
    
    public async Task<Result<AuthResultDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for LoginCommand: {Errors}", validationResult.Errors);
            return Result.Fail(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
        }
        
        var user = await _context.Users
            .Include(u => u.Roles)
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (user == null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            return Result.Fail("Invalid email or password.");
        }
        
        var accessToken = _jwtTokenGenerator.GenerateToken(user.Id, user.Email, user.Roles.Select(r => r.Name).ToList());
        var refreshToken = _refreshTokenGenerator.GenerateToken(user.Id);
        
        user.RefreshTokens.Add(refreshToken);
        _refreshTokenCleaner.CleanUp(user);
        
        await _context.SaveChangesAsync(cancellationToken);
        
        var authResult = new AuthResultDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token,
        };
        
        _logger.LogInformation("User {UserId} logged in successfully", user.Id);
        
        return Result.Ok(authResult);
    }
}