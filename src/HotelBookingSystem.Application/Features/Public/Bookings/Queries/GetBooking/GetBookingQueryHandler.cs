using FluentResults;
using FluentValidation;
using HotelBookingSystem.Application.Common.DTOs.Bookings;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using HotelBookingSystem.Application.Common.Interfaces.Users;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Application.Features.Public.Bookings.Queries.GetBooking;

public class GetBookingQueryHandler : IRequestHandler<GetBookingQuery, Result<BookingResponse>>
{
    private readonly IHotelBookingDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<GetBookingQuery> _bookingValidator;
    private readonly ILogger<GetBookingQueryHandler> _logger;

    public GetBookingQueryHandler(IHotelBookingDbContext context, ICurrentUserService currentUser, IValidator<GetBookingQuery> bookingValidator, ILogger<GetBookingQueryHandler> logger)
    {
        _context = context;
        _currentUser = currentUser;
        _bookingValidator = bookingValidator;
        _logger = logger;
    }

    public async Task<Result<BookingResponse>> Handle(GetBookingQuery request, CancellationToken cancellationToken)
    {
        var validationResult = await _bookingValidator.ValidateAsync(request, cancellationToken);
        
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Booking validation failed: {Errors}", validationResult.Errors);
            return Result.Fail(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
        }
        
        var userId = _currentUser.GetUserId();
        
        var booking = await _context.Bookings
            .Include(b => b.Room)
            .ThenInclude(r => r.Hotel)
            .Where(b => b.Id == request.BookingId && b.UserId == userId)
            .ProjectToType<BookingResponse>()
            .FirstOrDefaultAsync(cancellationToken);
        
        if (booking == null)
        {
            _logger.LogWarning("Booking with ID {BookingId} not found or not available for user {UserId}.", request.BookingId, userId);
            return Result.Fail(new Error("Booking not found or not available."));
        }
        
        return Result.Ok(booking);
    }
}