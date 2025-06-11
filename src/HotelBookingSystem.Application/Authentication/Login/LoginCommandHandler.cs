using FluentResults;
using HotelBookingSystem.Application.Authentication.Common;
using HotelBookingSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Authentication.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthResultDto>>
{
    private readonly IHotelBookingDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    
    public LoginCommandHandler(IHotelBookingDbContext context, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }
    
    public async Task<Result<AuthResultDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
        
        if (user == null || !_passwordHasher.Verify(request.Password, user.PasswordHash)) 
            return Result.Fail("Invalid email or password.");
        
        var token = _jwtTokenGenerator.GenerateToken(user.Id, user.Email, user.Roles.Select(r => r.Name).ToList());
        
        var authResult = new AuthResultDto
        {
            UserId = user.Id,
            Email = user.Email,
            Token = token,
        };
        
        return Result.Ok(authResult);
    }
}