using FluentResults;
using HotelBookingSystem.Application.Common.Interfaces.Authentication;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using HotelBookingSystem.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Application.Features.Bookings.Commands.ConfirmBooking;

public class ConfirmBookingCommandHandler : IRequestHandler<ConfirmBookingCommand, Result>
{
    private readonly IHotelBookingDbContext  _context;
    private readonly IJwtTokenValidator _jwtTokenValidator;
    private readonly ILogger<ConfirmBookingCommandHandler> _logger;

    public ConfirmBookingCommandHandler(IHotelBookingDbContext context, IJwtTokenValidator jwtTokenValidator, ILogger<ConfirmBookingCommandHandler> logger)
    {
        _context = context;
        _jwtTokenValidator = jwtTokenValidator;
        _logger = logger;
    }

    public async Task<Result> Handle(ConfirmBookingCommand request, CancellationToken cancellationToken)
    {
        var bookingId = _jwtTokenValidator.ValidateBookingConfirmationToken(request.Token);

        if (bookingId == null)
        {
            _logger.LogError("Invalid or expired token: {Token}", request.Token);
            return Result.Fail("Invalid or expired token");
        }
        
        var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == bookingId, cancellationToken);

        if (booking == null)
        {
            _logger.LogWarning("Booking not found: {BookingId}", bookingId);
            return Result.Fail("Booking not found");
        }

        if (booking.Status != BookingStatus.AwaitingUserConfirmation)
        {
            _logger.LogWarning("Booking already confirmed or invalid status: {Status}", booking.Status);
            return Result.Fail("Booking already confirmed");
        }

        booking.Status = BookingStatus.AwaitingManagerConfirmation;
        await _context.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Booking confirmed: {BookingId} by user {UserId}", bookingId,  booking.UserId);
        return Result.Ok();
    }
}