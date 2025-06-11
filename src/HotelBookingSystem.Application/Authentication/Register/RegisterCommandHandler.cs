using FluentResults;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Authentication.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<Guid>>
{
    private readonly IHotelBookingDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    
    public RegisterCommandHandler(IHotelBookingDbContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }
    
    public async Task<Result<Guid>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await _context.Users.AnyAsync(u => u.Email == request.Email, cancellationToken))
            return Result.Fail("Email is already taken.");

        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PasswordHash = _passwordHasher.Hash(request.Password),
            PhoneNumber = request.PhoneNumber,
            CreatedAt = DateTime.UtcNow,
            Roles = new List<Role> { new Role { Name = "User" } }
        };
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);
        
        return Result.Ok(user.Id);
    }
}