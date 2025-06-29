using FluentResults;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using HotelBookingSystem.Application.Common.Interfaces.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Application.Features.Profile.Hotels.Commands.UpdateHotel;

public class UpdateHotelCommandHandler : IRequestHandler<UpdateHotelCommand, Result>
{
    private readonly IHotelBookingDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly ILogger<UpdateHotelCommandHandler> _logger;
    
    public UpdateHotelCommandHandler(IHotelBookingDbContext context, ICurrentUserService currentUser, ILogger<UpdateHotelCommandHandler> logger)
    {
        _context = context;
        _currentUser = currentUser;
        _logger = logger;
    }

    public async Task<Result> Handle(UpdateHotelCommand request, CancellationToken cancellationToken)
    {
        var hotel = await _context.Hotels
            .FirstOrDefaultAsync(h => h.Id == request.Id, cancellationToken);
        
        if (hotel == null)
        {
            _logger.LogWarning("Hotel with ID {HotelId} not found", request.Id);
            return Result.Fail("Hotel not found.");
        }
        
        if (hotel.OwnerId != _currentUser.GetUserId())
        {
            _logger.LogWarning("User {UserId} attempted to update hotel {HotelId} without permission", _currentUser.GetUserId(), request.Id);
            return Result.Fail("You do not have permission to update this hotel.");
        }
        
        hotel.Name = request.Name;
        hotel.Description = request.Description;
        hotel.Address = request.Address;
        hotel.UpdatedAt = DateTime.UtcNow;
        hotel.IsApproved = false;
        hotel.IsVisible = false;
        
        await _context.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Hotel with ID {HotelId} has been updated by user {UserId}", request.Id, _currentUser.GetUserId());
        
        return Result.Ok();
    }
}