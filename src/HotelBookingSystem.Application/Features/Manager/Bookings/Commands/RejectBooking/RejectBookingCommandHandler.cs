using FluentResults;
using FluentValidation;
using HotelBookingSystem.Application.Common.DTOs.Bookings;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using HotelBookingSystem.Application.Common.Interfaces.Users;
using HotelBookingSystem.Domain.Enums;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Application.Features.Manager.Bookings.Commands.RejectBooking;

public class RejectBookingCommandHandler : IRequestHandler<RejectBookingCommand, Result<RejectBookingResponse>>
{
    private readonly IHotelBookingDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<RejectBookingCommand> _rejectBookingValidator;
    private readonly ILogger<RejectBookingCommandHandler> _logger;

    public RejectBookingCommandHandler(IHotelBookingDbContext context, ICurrentUserService currentUser, IValidator<RejectBookingCommand> rejectBookingValidator, ILogger<RejectBookingCommandHandler> logger)
    {
        _context = context;
        _currentUser = currentUser;
        _rejectBookingValidator = rejectBookingValidator;
        _logger = logger;
    }

    public async Task<Result<RejectBookingResponse>> Handle(RejectBookingCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _rejectBookingValidator.ValidateAsync(request, cancellationToken);
        
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for RejectBookingCommand: {Errors}", validationResult.Errors);
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
        
        if (booking.Room.Hotel.OwnerId != managerId)
        {
            _logger.LogWarning("User {UserId} does not have permission to reject booking with ID {BookingId}.", managerId, request.BookingId);
            return Result.Fail($"You do not have permission to reject this booking.");
        }
        
        if (booking.Status != BookingStatus.Pending)
        {
            _logger.LogWarning("Booking with ID {BookingId} is not in a pending state.", request.BookingId);
            return Result.Fail($"Booking with ID {request.BookingId} is not in a pending state.");
        }
        
        booking.Status = BookingStatus.Rejected;
        booking.RejectedAt = DateTime.UtcNow;
        booking.RejectionReason = request.RejectionReason;
        
        await _context.SaveChangesAsync(cancellationToken);
        
        var result = booking.Adapt<RejectBookingResponse>();
        
        _logger.LogInformation("Booking with ID {BookingId} has been rejected by manager {UserId}.", request.BookingId, managerId);
        
        return Result.Ok(result);
    }
}