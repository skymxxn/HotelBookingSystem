using FluentResults;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using HotelBookingSystem.Application.Common.Interfaces.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Application.Features.Profile.Hotels.Commands.PublishHotel;

public class PublishHotelCommandHandler : IRequestHandler<PublishHotelCommand, Result>
{
    private readonly IHotelBookingDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly ILogger<PublishHotelCommandHandler> _logger;
    
    public PublishHotelCommandHandler(IHotelBookingDbContext context, ICurrentUserService currentUser, ILogger<PublishHotelCommandHandler> logger)
    {
        _context = context;
        _currentUser = currentUser;
        _logger = logger;
    }
    
    public async Task<Result> Handle(PublishHotelCommand request, CancellationToken cancellationToken)
    {
        var managerId = _currentUser.GetUserId();
        var hotel = await _context.Hotels
            .FirstOrDefaultAsync(h => h.Id == request.HotelId && h.OwnerId == managerId, cancellationToken);

        if (hotel == null)
        {
            _logger.LogWarning("Hotel with ID {HotelId} not found or access denied for user {UserId}", request.HotelId, managerId);
            return Result.Fail(new Error("Hotel not found or access denied."));
        }

        if (hotel.IsVisible)
        {
            _logger.LogWarning("Hotel with ID {HotelId} is already published", request.HotelId);
            return Result.Fail(new Error("Hotel is already published."));
        }

        if (!hotel.IsApproved)
        {
            _logger.LogWarning("Hotel with ID {HotelId} must be approved before publishing", request.HotelId);
            return Result.Fail(new Error("Hotel must be approved before publishing."));
        }
        
        hotel.IsVisible = true;
        await _context.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Hotel with ID {HotelId} has been published by user {UserId}", request.HotelId, managerId);
        
        return Result.Ok();
    }
}