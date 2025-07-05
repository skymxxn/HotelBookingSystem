using FluentResults;
using FluentValidation;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using HotelBookingSystem.Application.Common.Interfaces.Users;
using HotelBookingSystem.Application.Features.Manager.Booking.DTOs;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Application.Features.Manager.Booking.Queries.GetBooking;

public class GetBookingQueryHandler : IRequestHandler<GetBookingQuery, Result<BookingResponse>>
{
    private readonly IHotelBookingDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<GetBookingQuery> _validator;
    private readonly ILogger<GetBookingQueryHandler> _logger;

    public GetBookingQueryHandler(IHotelBookingDbContext context, ICurrentUserService currentUser, IValidator<GetBookingQuery> validator, ILogger<GetBookingQueryHandler> logger)
    {
        _context = context;
        _currentUser = currentUser;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<BookingResponse>> Handle(GetBookingQuery request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for GetBookingQuery: {Errors}", validationResult.Errors);
            return Result.Fail(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
        }
        
        var managerId = _currentUser.GetUserId();
        
        var booking = await _context.Bookings
            .Include(b => b.Room)
            .ThenInclude(r => r.Hotel)
            .Where(b => b.Id == request.BookingId && b.Room.Hotel.OwnerId == managerId)
            .ProjectToType<BookingResponse>()
            .FirstOrDefaultAsync(cancellationToken);
        
        if (booking == null)
        {
            _logger.LogWarning("Booking with ID {BookingId} not found for manager {ManagerId}.", request.BookingId, managerId);
            return Result.Fail($"Booking with ID {request.BookingId} not found.");
        }
        
        return Result.Ok(booking);
    }
}