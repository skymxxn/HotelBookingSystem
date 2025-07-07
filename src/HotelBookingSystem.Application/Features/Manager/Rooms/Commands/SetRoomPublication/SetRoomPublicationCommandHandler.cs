using FluentResults;
using FluentValidation;
using HotelBookingSystem.Application.Common.DTOs.Rooms;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using HotelBookingSystem.Application.Common.Interfaces.Users;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Application.Features.Manager.Rooms.Commands.SetRoomPublication;

public class SetRoomPublicationCommandHandler : IRequestHandler<SetRoomPublicationCommand, Result<RoomResponse>>
{
    private readonly IHotelBookingDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<SetRoomPublicationCommand> _validator;
    private readonly ILogger<SetRoomPublicationCommandHandler> _logger;
    
    public SetRoomPublicationCommandHandler(IHotelBookingDbContext context, ILogger<SetRoomPublicationCommandHandler> logger, ICurrentUserService currentUser, IValidator<SetRoomPublicationCommand> validator)
    {
        _context = context;
        _logger = logger;
        _currentUser = currentUser;
        _validator = validator;
    }
    
    public async Task<Result<RoomResponse>> Handle(SetRoomPublicationCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for SetRoomPublicationCommand: {Errors}", validationResult.Errors);
            return Result.Fail(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
        }
        
        var managerId = _currentUser.GetUserId();
        var room = await _context.Rooms
            .FirstOrDefaultAsync(r =>
                    r.Id == request.RoomId &&
                    r.Hotel.OwnerId == managerId,
                cancellationToken);
        
        if (room == null)
        {
            _logger.LogWarning("Room with ID {RoomId} not found or access denied for user {UserId}", request.RoomId, managerId);
            return Result.Fail(new Error("Room not found or access denied."));
        }

        if (room.IsPublished == request.IsPublished)
        {
            _logger.LogInformation("Room with ID {RoomId} publication status is already set to {IsPublished} for user {UserId}", request.RoomId, request.IsPublished, managerId);
            return Result.Ok();
        }

        room.IsPublished = request.IsPublished;
        await _context.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Room with ID {RoomId} publication status set to {IsPublished} by user {UserId}", request.RoomId, request.IsPublished, managerId);
        
        return Result.Ok(room.Adapt<RoomResponse>());
    }
}