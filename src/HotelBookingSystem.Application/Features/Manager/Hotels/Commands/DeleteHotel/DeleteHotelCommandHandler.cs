using FluentResults;
using FluentValidation;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using HotelBookingSystem.Application.Common.Interfaces.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Application.Features.Manager.Hotels.Commands.DeleteHotel;

public class DeleteHotelCommandHandler : IRequestHandler<DeleteHotelCommand, Result>
{
    private readonly IHotelBookingDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<DeleteHotelCommand> _validator;
    private readonly ILogger<DeleteHotelCommandHandler> _logger;
    
    public DeleteHotelCommandHandler(IHotelBookingDbContext context, ICurrentUserService currentUser, ILogger<DeleteHotelCommandHandler> logger, IValidator<DeleteHotelCommand> validator)
    {
        _context = context;
        _currentUser = currentUser;
        _logger = logger;
        _validator = validator;
    }
    
    public async Task<Result> Handle(DeleteHotelCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for DeleteHotelCommand: {Errors}", validationResult.Errors);
            return Result.Fail(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
        }
        
        var hotel = await _context.Hotels
            .FirstOrDefaultAsync(h => h.Id == request.Id, cancellationToken);
        
        if (hotel == null)
        {
            _logger.LogWarning("Hotel with ID {HotelId} not found", request.Id);
            return Result.Fail("Hotel not found.");
        }
        
        if (hotel.OwnerId != _currentUser.GetUserId())
        {
            _logger.LogWarning("User {UserId} attempted to delete hotel {HotelId} without permission", _currentUser.GetUserId(), request.Id);
            return Result.Fail("You do not have permission to delete this hotel.");
        }
        
        _context.Hotels.Remove(hotel);
        await _context.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Hotel with ID {HotelId} has been deleted by user {UserId}", request.Id, _currentUser.GetUserId());
        
        return Result.Ok();
    }
}