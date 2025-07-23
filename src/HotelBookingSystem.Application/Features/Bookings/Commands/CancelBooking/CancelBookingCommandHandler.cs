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

namespace HotelBookingSystem.Application.Features.Bookings.Commands.CancelBooking;

public class CancelBookingCommandHandler : IRequestHandler<CancelBookingCommand, Result<CancelBookingResponse>>
{
    private readonly IHotelBookingDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<CancelBookingCommand> _bookingValidator;
    private readonly ILogger<CancelBookingCommandHandler> _logger;

    public CancelBookingCommandHandler(IHotelBookingDbContext context, ICurrentUserService currentUser, ILogger<CancelBookingCommandHandler> logger, IValidator<CancelBookingCommand> bookingValidator)
    {
        _context = context;
        _currentUser = currentUser;
        _logger = logger;
        _bookingValidator = bookingValidator;
    }

    public async Task<Result<CancelBookingResponse>> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _bookingValidator.ValidateAsync(request, cancellationToken);
        
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for CancelBookingCommand: {Errors}", validationResult.Errors);
            return Result.Fail(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
        }
        
        var userId = _currentUser.GetUserId();
        
        var booking = await _context.Bookings
            .FirstOrDefaultAsync(b => b.Id == request.BookingId && b.UserId == userId, cancellationToken);
        
        if (booking == null)
        {
            _logger.LogWarning("Booking with ID {BookingId} not found for user {UserId}.", request.BookingId, userId);
            return Result.Fail($"Booking with ID {request.BookingId} not found.");
        }

        if (booking.Status == BookingStatus.Cancelled)
        {
            _logger.LogInformation("Booking with ID {BookingId} is already cancelled.", request.BookingId);
            return Result.Ok(booking.Adapt<CancelBookingResponse>());
        }
        
        booking.Status = BookingStatus.Cancelled;
        booking.CancelledAt = DateTime.UtcNow;
        booking.CancellationReason = request.Reason;
        
        await _context.SaveChangesAsync(cancellationToken);
        
        var response = booking.Adapt<CancelBookingResponse>();
        
        _logger.LogInformation("Booking with ID {BookingId} has been cancelled successfully for user {UserId}.", request.BookingId, userId);
        
        return Result.Ok(response);
    }
}