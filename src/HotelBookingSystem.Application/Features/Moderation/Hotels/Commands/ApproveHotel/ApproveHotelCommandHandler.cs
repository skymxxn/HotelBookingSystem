using FluentResults;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using HotelBookingSystem.Application.Common.Interfaces.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Application.Features.Moderation.Hotels.Commands.ApproveHotel;

public class ApproveHotelCommandHandler : IRequestHandler<ApproveHotelCommand, Result>
{
    private readonly IHotelBookingDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly ILogger<ApproveHotelCommandHandler> _logger;
    
    public ApproveHotelCommandHandler(IHotelBookingDbContext context, ILogger<ApproveHotelCommandHandler> logger, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
        _logger = logger;
    }
    
    public async Task<Result> Handle(ApproveHotelCommand request, CancellationToken cancellationToken)
    {
        var hotel = await _context.Hotels
            .FirstOrDefaultAsync(h => h.Id == request.HotelId, cancellationToken);

        if (hotel == null)
        {
            _logger.LogWarning("Hotel with ID {HotelId} not found", request.HotelId);
            return Result.Fail("Hotel not found.");
        }

        if (hotel.IsApproved)
        {
            _logger.LogWarning("Hotel with ID {HotelId} is already approved", request.HotelId);
            return Result.Fail("Hotel is already approved.");
        }
        
        hotel.IsApproved = true;
        await _context.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Hotel with ID {HotelId} has been approved by moderator {ModeratorId}", request.HotelId, _currentUser.GetUserId());
        
        return Result.Ok();
    }
}