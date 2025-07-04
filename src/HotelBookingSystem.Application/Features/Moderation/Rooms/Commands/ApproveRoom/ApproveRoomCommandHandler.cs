using FluentResults;
using FluentValidation;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using HotelBookingSystem.Application.Common.Interfaces.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Application.Features.Moderation.Rooms.Commands.ApproveRoom;

public class ApproveRoomCommandHandler : IRequestHandler<ApproveRoomCommand, Result<Guid>>
{
    private readonly IHotelBookingDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<ApproveRoomCommand> _validator;
    private readonly ILogger<ApproveRoomCommandHandler> _logger;
    
    public ApproveRoomCommandHandler(IHotelBookingDbContext context, IValidator<ApproveRoomCommand> validator, ILogger<ApproveRoomCommandHandler> logger, ICurrentUserService currentUser)
    {
        _context = context;
        _validator = validator;
        _logger = logger;
        _currentUser = currentUser;
    }

    public async Task<Result<Guid>> Handle(ApproveRoomCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for ApproveRoomCommand: {Errors}", validationResult.Errors);
            return Result.Fail(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
        }
        
        var moderatorId = _currentUser.GetUserId();

        var room = await _context.Rooms
            .Include(r => r.Hotel)
            .FirstOrDefaultAsync(r => r.Id == request.RoomId && r.HotelId == request.HotelId, cancellationToken);
        
        if (room == null)
        {
            _logger.LogWarning("Room with ID {RoomId} in hotel {HotelId} not found", request.RoomId, request.HotelId);
            return Result.Fail(new Error("Room not found."));
        }
        
        if (room.IsApproved)
        {
            _logger.LogWarning("Room with ID {RoomId} in hotel {HotelId} is already approved", request.RoomId, request.HotelId);
            return Result.Fail(new Error("Room is already approved."));
        }

        room.IsApproved = true;
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Room with ID {RoomId} in hotel {HotelId} approved successfully by moderator {ModeratorId}", 
            request.RoomId, request.HotelId, moderatorId);
        
        return Result.Ok(room.Id);
    }
}