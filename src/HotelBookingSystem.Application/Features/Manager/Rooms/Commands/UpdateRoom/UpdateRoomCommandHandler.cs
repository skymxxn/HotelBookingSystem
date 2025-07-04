using FluentResults;
using FluentValidation;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using HotelBookingSystem.Application.Common.Interfaces.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Application.Features.Manager.Rooms.Commands.UpdateRoom;

public class UpdateRoomCommandHandler : IRequestHandler<UpdateRoomCommand, Result>
{
    private readonly IHotelBookingDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<UpdateRoomCommand> _validator;
    private readonly ILogger<UpdateRoomCommandHandler> _logger;

    public UpdateRoomCommandHandler(IHotelBookingDbContext context, ICurrentUserService currentUser, IValidator<UpdateRoomCommand> validator, ILogger<UpdateRoomCommandHandler> logger)
    {
        _context = context;
        _currentUser = currentUser;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for UpdateRoomCommand: {Errors}", validationResult.Errors);
            return Result.Fail(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
        }

        var managerId = _currentUser.GetUserId();
        var room = await _context.Rooms
            .FirstOrDefaultAsync(r => r.Id == request.RoomId && r.Hotel.OwnerId == managerId, cancellationToken);

        if (room == null)
        {
            _logger.LogWarning("Room with ID {RoomId} not found or access denied for user {UserId}", request.RoomId, managerId);
            return Result.Fail(new Error("Room not found or access denied."));
        }
        
        room.Name = request.Name;
        room.Description = request.Description;
        room.PricePerNight = request.PricePerNight;
        room.Capacity = request.Capacity;
        room.UpdatedAt = DateTime.UtcNow;
        
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Room with ID {RoomId} updated by user {UserId}", request.RoomId, managerId);
        
        return Result.Ok();
    }
}