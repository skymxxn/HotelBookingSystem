using FluentResults;
using HotelBookingSystem.Application.Common.Interfaces.Authentication;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using HotelBookingSystem.Application.Features.Authentication.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.Authentication.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<AuthResultDto>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IHotelBookingDbContext _context;
    private readonly IRefreshTokenCleaner _refreshTokenCleaner;
    
    public RefreshTokenCommandHandler(IJwtTokenGenerator jwtTokenGenerator, IHotelBookingDbContext context, IRefreshTokenGenerator refreshTokenGenerator, IRefreshTokenCleaner refreshTokenCleaner)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _context = context;
        _refreshTokenGenerator = refreshTokenGenerator;
        _refreshTokenCleaner = refreshTokenCleaner;
    }
    
    public async Task<Result<AuthResultDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await _context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken, cancellationToken);

        if (refreshToken == null || refreshToken.IsRevoked)
        {
            return await Task.FromResult(Result.Fail<AuthResultDto>("Invalid refresh token."));
        }
        
        if (refreshToken.ExpiresAt < DateTime.UtcNow)
        {
            return await Task.FromResult(Result.Fail<AuthResultDto>("Refresh token has expired."));
        }
        
        var user = await _context.Users
            .Include(u => u.RefreshTokens)
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Id == refreshToken.UserId,cancellationToken);
        
        if (user == null)
        {
            return await Task.FromResult(Result.Fail<AuthResultDto>("Refresh token does not belong to a valid user."));
        }
        
        refreshToken.IsRevoked = true;
        
        var newRefreshToken = _refreshTokenGenerator.GenerateToken(user.Id);
        user.RefreshTokens.Add(newRefreshToken);
        
        _refreshTokenCleaner.CleanUp(user);
        
        var accessToken = _jwtTokenGenerator.GenerateToken(user.Id, user.Email, user.Roles.Select(r => r.Name).ToList());
        
        await _context.SaveChangesAsync(cancellationToken);
        
        var authResult = new AuthResultDto
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken.Token,
        };
        
        return Result.Ok(authResult);
    }
}