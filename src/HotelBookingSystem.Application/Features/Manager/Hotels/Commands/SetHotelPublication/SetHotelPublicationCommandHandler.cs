using FluentResults;
using FluentValidation;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using HotelBookingSystem.Application.Common.Interfaces.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Application.Features.Manager.Hotels.Commands.SetHotelPublication;

public class SetHotelPublicationCommandHandler : IRequestHandler<SetHotelPublicationCommand, Result>
{
    private readonly IHotelBookingDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<SetHotelPublicationCommand> _validator;
    private readonly ILogger<SetHotelPublicationCommandHandler> _logger;
    
    public SetHotelPublicationCommandHandler(IHotelBookingDbContext context, ICurrentUserService currentUser, ILogger<SetHotelPublicationCommandHandler> logger, IValidator<SetHotelPublicationCommand> validator)
    {
        _context = context;
        _currentUser = currentUser;
        _logger = logger;
        _validator = validator;
    }
    
    public async Task<Result> Handle(SetHotelPublicationCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for SetHotelPublicationCommand: {Errors}", validationResult.Errors);
            return Result.Fail(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
        }
        
        var managerId = _currentUser.GetUserId();
        var hotel = await _context.Hotels
            .FirstOrDefaultAsync(h => h.Id == request.HotelId && h.OwnerId == managerId, cancellationToken);

        if (hotel == null)
        {
            _logger.LogWarning("Hotel with ID {HotelId} not found or access denied for user {UserId}", request.HotelId, managerId);
            return Result.Fail(new Error("Hotel not found or access denied."));
        }

        if (!hotel.IsApproved)
        {
            _logger.LogWarning("Hotel with ID {HotelId} is not approved and cannot be published by user {UserId}", request.HotelId, managerId);
            return Result.Fail(new Error("Hotel is not approved and cannot be published."));
        }
        
        if (hotel.IsPublished == request.IsPublished)
        {
            _logger.LogWarning("Hotel with ID {HotelId} publication status is already set to {IsPublished} for user {UserId}", request.HotelId, request.IsPublished, managerId);
            return Result.Fail(new Error($"Hotel publication status is already set to {request.IsPublished}."));
        }

        hotel.IsPublished = request.IsPublished;
        await _context.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Hotel with ID {HotelId} publication status set to {IsPublished} by user {UserId}", request.HotelId, request.IsPublished, managerId);
        
        return Result.Ok();
    }
}