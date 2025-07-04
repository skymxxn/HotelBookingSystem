using FluentResults;
using FluentValidation;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using HotelBookingSystem.Application.Common.Interfaces.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Application.Features.Manager.Rooms.Commands.DeleteRoom;

public class DeleteRoomCommandHandler : IRequestHandler<DeleteRoomCommand, Result>
{
    private readonly IHotelBookingDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<DeleteRoomCommand> _validator;
    private readonly ILogger<DeleteRoomCommandHandler> _logger;

    public DeleteRoomCommandHandler(IHotelBookingDbContext context, ICurrentUserService currentUser, IValidator<DeleteRoomCommand> validator, ILogger<DeleteRoomCommandHandler> logger)
    {
        _context = context;
        _currentUser = currentUser;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result> Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for DeleteRoomCommand: {Errors}", validationResult.Errors);
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

        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Room with ID {RoomId} deleted by user {UserId}", request.RoomId, managerId);
        
        return Result.Ok();
    }
}