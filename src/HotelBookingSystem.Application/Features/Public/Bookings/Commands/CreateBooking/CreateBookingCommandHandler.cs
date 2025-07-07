using FluentResults;
using FluentValidation;
using HotelBookingSystem.Application.Common.DTOs.Bookings;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using HotelBookingSystem.Application.Common.Interfaces.Users;
using HotelBookingSystem.Domain.Entities;
using HotelBookingSystem.Domain.Enums;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Application.Features.Public.Bookings.Commands.CreateBooking;

public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, Result<CreateBookingResponse>>
{
    private readonly IHotelBookingDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<CreateBookingCommand> _bookingValidator;
    private readonly ILogger<CreateBookingCommandHandler> _logger;

    public CreateBookingCommandHandler(IHotelBookingDbContext context, ICurrentUserService currentUser, IValidator<CreateBookingCommand> bookingValidator, ILogger<CreateBookingCommandHandler> logger)
    {
        _context = context;
        _currentUser = currentUser;
        _bookingValidator = bookingValidator;
        _logger = logger;
    }

    public async Task<Result<CreateBookingResponse>> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _bookingValidator.ValidateAsync(request, cancellationToken);
        
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Booking validation failed: {Errors}", validationResult.Errors);
            return Result.Fail(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
        }
        
        var userId = _currentUser.GetUserId();
        
        var room = await _context.Rooms
            .Include(r => r.Hotel)
            .FirstOrDefaultAsync(r => r.Id == request.RoomId && r.IsPublished && r.Hotel.IsPublished, cancellationToken);

        if (room == null)
        {
            _logger.LogWarning("Room with ID {RoomId} not found or not available.", request.RoomId);
            return Result.Fail(new Error("Room not found or not available."));
        }
        
        var overlap = await _context.Bookings
            .AnyAsync(b => b.RoomId == request.RoomId &&
                           b.Status != BookingStatus.Rejected &&
                           b.Status != BookingStatus.Cancelled &&
                           b.FromDate < request.ToDate &&
                           b.ToDate > request.FromDate, cancellationToken);
        
        if (overlap) 
        {
            _logger.LogWarning("Booking overlap detected for Room ID {RoomId} from {FromDate} to {ToDate}.", request.RoomId, request.FromDate, request.ToDate);
            return Result.Fail(new Error("The room is already booked for the selected dates."));
        }

        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            RoomId = request.RoomId,
            UserId = userId,
            FromDate = request.FromDate,
            ToDate = request.ToDate,
            Description = request.Description,
            Status = BookingStatus.Pending,
            TotalPrice = room.PricePerNight * (decimal)(request.ToDate - request.FromDate).TotalDays,
            CreatedAt = DateTime.UtcNow
        };
        
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Booking created successfully with ID {BookingId} by user {UserId} for Room ID {RoomId}.", booking.Id, userId, request.RoomId);
        
        var response = booking.Adapt<CreateBookingResponse>();
        
        return Result.Ok(response);
    }
}