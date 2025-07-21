using FluentResults;
using HotelBookingSystem.Application.Common.Interfaces.Authentication;
using HotelBookingSystem.Application.Common.Interfaces.Email;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Application.Features.Authentication.Commands.ForgotPassword;

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Result>
{
    private readonly IHotelBookingDbContext  _context;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IEmailService  _emailService;
    private readonly ILogger<ForgotPasswordCommandHandler> _logger;

    public ForgotPasswordCommandHandler(IHotelBookingDbContext context, IJwtTokenGenerator jwtTokenGenerator, ILogger<ForgotPasswordCommandHandler> logger, IEmailService emailService)
    {
        _context = context;
        _jwtTokenGenerator = jwtTokenGenerator;
        _logger = logger;
        _emailService = emailService;
    }

    public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (user == null)
        {
            _logger.LogWarning("User with {Email} not found", request.Email);
            return Result.Ok();
        }
        
        var token = _jwtTokenGenerator.GeneratePasswordResetToken(user.Id);
        await _emailService.SendPasswordResetAsync(user.Email, token);
        
        return Result.Ok();
    }
}