using FluentResults;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using HotelBookingSystem.Application.Common.Interfaces.Users;
using HotelBookingSystem.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Application.Features.Bookings.Commands.UpdateBooking;

public class UpdateBookingCommandHandler : IRequestHandler<UpdateBookingCommand, Result>
{
    private readonly IHotelBookingDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly ILogger<UpdateBookingCommandHandler> _logger;

    public UpdateBookingCommandHandler(IHotelBookingDbContext context, ICurrentUserService currentUser, ILogger<UpdateBookingCommandHandler> logger)
    {
        _context = context;
        _currentUser = currentUser;
        _logger = logger;
    }

    public async Task<Result> Handle(UpdateBookingCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.GetUserId();
        var booking = await _context.Bookings.FirstOrDefaultAsync(
            b => b.Id == request.BookingId && b.UserId == userId,
            cancellationToken);

        if (booking == null)
        {
            _logger.LogWarning("Booking with ID {BookingId} not found for user {UserId}.", request.BookingId, userId);
            return Result.Fail("Booking not found.");
        }

        if (booking.Status != BookingStatus.AwaitingUserConfirmation && booking.Status != BookingStatus.AwaitingManagerConfirmation)
        {
            _logger.LogWarning("Booking with ID {BookingId} cannot be updated because it is in status {Status}.", request.BookingId, booking.Status);
            return Result.Fail("Booking cannot be updated in its current status.");
        }

        booking.FromDate = request.FromDate;
        booking.ToDate = request.ToDate;
        if (!string.IsNullOrWhiteSpace(request.Description))
        {
            booking.Description = request.Description;
        }

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Booking with ID {BookingId} successfully updated by user {UserId}.", request.BookingId, userId);

        return Result.Ok();
    }
}
