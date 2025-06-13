using FluentResults;
using HotelBookingSystem.Application.Authentication.Common;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Application.Common.Interfaces.Authentication;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Authentication.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthResultDto>>
{
    private readonly IHotelBookingDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IRefreshTokenCleaner _refreshTokenCleaner;
    
    public LoginCommandHandler(IHotelBookingDbContext context, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator, IRefreshTokenGenerator refreshTokenGenerator, IRefreshTokenCleaner refreshTokenCleaner)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
        _refreshTokenCleaner = refreshTokenCleaner;
    }
    
    public async Task<Result<AuthResultDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(u => u.Roles)
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
        
        if (user == null || !_passwordHasher.Verify(request.Password, user.PasswordHash)) 
            return Result.Fail("Invalid email or password.");
        
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
        
        return Result.Ok(authResult);
    }
}