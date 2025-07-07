using FluentResults;
using FluentValidation;
using HotelBookingSystem.Application.Common.DTOs;
using HotelBookingSystem.Application.Common.DTOs.Rooms;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using HotelBookingSystem.Application.Common.Interfaces.Users;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Application.Features.Manager.Rooms.Queries.GetRoom;

public class GetRoomQueryHandler : IRequestHandler<GetRoomQuery, Result<RoomResponse>>
{
    private readonly IHotelBookingDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<GetRoomQuery> _validator;
    private readonly ILogger<GetRoomQueryHandler> _logger;

    public GetRoomQueryHandler(IHotelBookingDbContext context, ICurrentUserService currentUser, ILogger<GetRoomQueryHandler> logger, IValidator<GetRoomQuery> validator)
    {
        _context = context;
        _currentUser = currentUser;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Result<RoomResponse>> Handle(GetRoomQuery request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for GetRoomQuery: {Errors}", validationResult.Errors);
            return Result.Fail(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
        }
        
        var managerId = _currentUser.GetUserId();

        var room = await _context.Rooms
            .Where(r => r.Id == request.RoomId && r.Hotel.OwnerId == managerId)
            .ProjectToType<RoomResponse>()
            .FirstOrDefaultAsync(cancellationToken);

        if (room == null)
        {
            _logger.LogWarning("Room with ID {RoomId} not found or access denied for user {UserId}", request.RoomId, managerId);
            return Result.Fail(new Error("Room not found or access denied."));
        }
        
        return Result.Ok(room);
    }
}