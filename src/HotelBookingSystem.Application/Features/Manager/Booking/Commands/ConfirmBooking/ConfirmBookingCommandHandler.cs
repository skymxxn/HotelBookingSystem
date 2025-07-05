using FluentResults;
using FluentValidation;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using HotelBookingSystem.Application.Common.Interfaces.Users;
using HotelBookingSystem.Application.Features.Manager.Booking.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Application.Features.Manager.Booking.Commands.ConfirmBooking;

public class ConfirmBookingCommandHandler : IRequestHandler<ConfirmBookingCommand, Result<ConfirmBookingResponse>>
{
    private readonly IHotelBookingDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<ConfirmBookingCommand> _confirmBookingValidator;
    private readonly ILogger<ConfirmBookingCommandHandler> _logger;

    public ConfirmBookingCommandHandler(IHotelBookingDbContext context, ICurrentUserService currentUserService, IValidator<ConfirmBookingCommand> confirmBookingValidator, ILogger<ConfirmBookingCommandHandler> logger)
    {
        _context = context;
        _currentUser = currentUserService;
        _confirmBookingValidator = confirmBookingValidator;
        _logger = logger;
    }

    public async Task<Result<ConfirmBookingResponse>> Handle(ConfirmBookingCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _confirmBookingValidator.ValidateAsync(request, cancellationToken);
        
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for ConfirmBookingCommand: {Errors}", validationResult.Errors);
            return Result.Fail(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
        }
        
        var managerId = _currentUser.GetUserId();
        
        var booking = await _context.Bookings
            .Include(b => b.Room)
            .ThenInclude(r => r.Hotel)
            .FirstOrDefaultAsync(b =>
                b.Id == request.BookingId &&
                b.Room.Hotel.OwnerId == managerId, cancellationToken);
        
        if (booking == null)
        {
            _logger.LogWarning("Booking with ID {BookingId} not found.", request.BookingId);
            return Result.Fail($"Booking with ID {request.BookingId} not found.");
        }
        
        if (booking.Status != Domain.Enums.BookingStatus.Pending)
        {
            _logger.LogWarning("Booking with ID {BookingId} is not in a pending state.", request.BookingId);
            return Result.Fail($"Booking with ID {request.BookingId} is not in a pending state.");
        }
        
        var confirmationTime = DateTime.UtcNow;
        booking.Status = Domain.Enums.BookingStatus.Confirmed;
        booking.ConfirmedAt = confirmationTime;
        
        await _context.SaveChangesAsync(cancellationToken);

        var result = new ConfirmBookingResponse
        {
            BookingId = booking.Id,
            Status = booking.Status.ToString(),
            ConfirmationDate = confirmationTime,
        };
        
        _logger.LogInformation("Booking with ID {BookingId} has been confirmed by user {UserId}.", request.BookingId, managerId);
        
        return Result.Ok(result);
    }
}