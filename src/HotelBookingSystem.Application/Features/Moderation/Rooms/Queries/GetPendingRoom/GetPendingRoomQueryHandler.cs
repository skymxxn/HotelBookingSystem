using FluentResults;
using FluentValidation;
using HotelBookingSystem.Application.Common.DTOs;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Application.Features.Moderation.Rooms.Queries.GetPendingRoom;

public class GetPendingRoomQueryHandler : IRequestHandler<GetPendingRoomQuery, Result<RoomResponse>>
{
    private readonly IHotelBookingDbContext _context;
    private readonly IValidator<GetPendingRoomQuery> _validator;
    private readonly ILogger<GetPendingRoomQueryHandler> _logger;

    public GetPendingRoomQueryHandler(IHotelBookingDbContext context, IValidator<GetPendingRoomQuery> validator, ILogger<GetPendingRoomQueryHandler> logger)
    {
        _context = context;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<RoomResponse>> Handle(GetPendingRoomQuery request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for GetPendingRoomQuery: {Errors}", validationResult.Errors);
            return Result.Fail(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
        }

        var room = await _context.Rooms
            .Where(r => r.Id == request.RoomId && r.HotelId == request.HotelId && !r.IsApproved)
            .ProjectToType<RoomResponse>()
            .FirstOrDefaultAsync(cancellationToken);

        if (room == null)
        {
            _logger.LogWarning("Pending room with ID {RoomId} not found in hotel {HotelId}", request.RoomId, request.HotelId);
            return Result.Fail(new Error("Pending room not found."));
        }

        return Result.Ok(room);
    }
}