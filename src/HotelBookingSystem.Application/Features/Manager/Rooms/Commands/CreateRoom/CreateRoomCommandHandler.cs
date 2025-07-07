using FluentResults;
using FluentValidation;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using HotelBookingSystem.Application.Common.Interfaces.Users;
using HotelBookingSystem.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Application.Features.Manager.Rooms.Commands.CreateRoom;

public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, Result<Guid>>
{
    private readonly IHotelBookingDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<CreateRoomCommand> _validator;
    private readonly ILogger<CreateRoomCommandHandler> _logger;

    public CreateRoomCommandHandler(IHotelBookingDbContext context, ILogger<CreateRoomCommandHandler> logger, ICurrentUserService currentUser, IValidator<CreateRoomCommand> validator)
    {
        _context = context;
        _logger = logger;
        _currentUser = currentUser;
        _validator = validator;
    }

    public async Task<Result<Guid>> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Capacity: {Capacity}, Price: {Price}", request.Capacity, request.PricePerNight);

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for CreateRoomCommand: {Errors}", validationResult.Errors);
            return Result.Fail(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
        }
        
        var hotel = await _context.Hotels.
            FirstOrDefaultAsync(h => h.Id == request.HotelId, cancellationToken);
        
        if (hotel == null)
        {
            _logger.LogWarning("Hotel with ID {HotelId} not found", request.HotelId);
            return Result.Fail("Hotel not found.");
        }

        var room = new Room
        {
            HotelId = request.HotelId,
            Name = request.Name,
            Description = request.Description,
            PricePerNight = request.PricePerNight,
            Capacity = request.Capacity,
            CreatedAt = DateTime.UtcNow
        };

        _context.Rooms.Add(room);
        await _context.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Room with ID {RoomId} created in hotel {HotelId} by user {UserId}", room.Id, request.HotelId, _currentUser.GetUserId());

        return Result.Ok(room.Id);
    }
}