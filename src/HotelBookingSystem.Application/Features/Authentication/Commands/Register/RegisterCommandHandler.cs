using FluentResults;
using FluentValidation;
using HotelBookingSystem.Application.Common.Interfaces.Authentication;
using HotelBookingSystem.Application.Common.Interfaces.Email;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using HotelBookingSystem.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Application.Features.Authentication.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<string>>
{
    private readonly IHotelBookingDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IValidator<RegisterCommand> _validator;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IEmailService _emailService;
    private readonly ILogger<RegisterCommandHandler> _logger;
    
    public RegisterCommandHandler(IHotelBookingDbContext context, IPasswordHasher passwordHasher, IValidator<RegisterCommand> validator, ILogger<RegisterCommandHandler> logger, IJwtTokenGenerator jwtTokenGenerator, IEmailService emailService)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _validator = validator;
        _logger = logger;
        _jwtTokenGenerator = jwtTokenGenerator;
        _emailService = emailService;
    }
    
    public async Task<Result<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for RegisterCommand: {Errors}", validationResult.Errors);
            return Result.Fail(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
        }

        if (await _context.Users.AnyAsync(u => u.Email == request.Email, cancellationToken))
        {
            _logger.LogWarning("Email {Email} is already taken", request.Email);
            return Result.Fail("Email is already taken.");
        }
        
        if (!Guid.TryParse(request.RoleId, out var roleGuid))
        {
            _logger.LogWarning("Invalid RoleId format: {RoleId}", request.RoleId);
            return Result.Fail("Role not found.");
        }
        
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == roleGuid, cancellationToken);
        
        if (role == null)
        {
            _logger.LogWarning("Role with ID {RoleId} not found", request.RoleId);
            return Result.Fail("Role not found.");
        }
        
        if (role.Name != "User" && role.Name != "Manager")
        {
            _logger.LogWarning("Invalid role {RoleName} for registration", role.Name);
            return Result.Fail("Invalid role for registration.");
        }

        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PasswordHash = _passwordHasher.Hash(request.Password),
            PhoneNumber = request.PhoneNumber,
            CreatedAt = DateTime.UtcNow,
            Roles = new List<Role> { role }
        };
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);
        
        var confirmationToken = _jwtTokenGenerator.GenerateEmailVerificationToken(user.Id);
        await _emailService.SendEmailConfirmationAsync(request.Email, confirmationToken);

        return Result.Ok("Registration successful. Email verification sent.");
    }
}