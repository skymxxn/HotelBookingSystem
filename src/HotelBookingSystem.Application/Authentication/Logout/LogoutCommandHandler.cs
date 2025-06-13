using FluentResults;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Authentication.Logout;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result>
{
    private readonly IHotelBookingDbContext _context;
    
    public LogoutCommandHandler(IHotelBookingDbContext context)
    {
        _context = context;
    }
    
    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(t => t.Token == request.RefreshToken, cancellationToken);
        
        if (refreshToken == null)
        {
            return Result.Fail("Invalid refresh token.");
        }
        
        refreshToken.IsRevoked = true;
        await _context.SaveChangesAsync(cancellationToken);
        
        return Result.Ok();
    }
}