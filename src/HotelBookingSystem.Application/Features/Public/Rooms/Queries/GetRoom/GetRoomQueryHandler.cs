using FluentResults;
using FluentValidation;
using HotelBookingSystem.Application.Common.DTOs;
using HotelBookingSystem.Application.Common.DTOs.Rooms;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Application.Features.Public.Rooms.Queries.GetRoom;

public class GetRoomQueryHandler : IRequestHandler<GetRoomQuery, Result<RoomResponse>>
{
    private readonly IHotelBookingDbContext _context;
    private readonly IValidator<GetRoomQuery> _validator;
    private readonly ILogger<GetRoomQueryHandler> _logger;
    
    public GetRoomQueryHandler(IHotelBookingDbContext context, ILogger<GetRoomQueryHandler> logger, IValidator<GetRoomQuery> validator)
    {
        _context = context;
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
        
        var room = await _context.Rooms
            .Where(r =>
                r.Id == request.RoomId &&
                r.HotelId == request.HotelId &&
                r.IsPublished &&
                r.IsApproved &&
                r.Hotel.IsPublished)
            .ProjectToType<RoomResponse>()
            .FirstOrDefaultAsync(cancellationToken);

        if (room == null)
        {
            _logger.LogWarning("Room with ID {RoomId} not found or not published", request.RoomId);
            return Result.Fail(new Error("Room not found or not published."));
        }

        return room;
    }
}