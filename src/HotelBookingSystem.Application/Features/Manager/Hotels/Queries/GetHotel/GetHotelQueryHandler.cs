using FluentResults;
using FluentValidation;
using HotelBookingSystem.Application.Common.DTOs;
using HotelBookingSystem.Application.Common.DTOs.Hotels;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using HotelBookingSystem.Application.Common.Interfaces.Users;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Application.Features.Manager.Hotels.Queries.GetHotel;

public class GetHotelQueryHandler : IRequestHandler<GetHotelQuery, Result<HotelResponse>>
{
    private readonly IHotelBookingDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<GetHotelQuery> _validator;
    private readonly ILogger<GetHotelQueryHandler> _logger;
    
    public GetHotelQueryHandler(IHotelBookingDbContext context, ICurrentUserService currentUser, ILogger<GetHotelQueryHandler> logger, IValidator<GetHotelQuery> validator)
    {
        _context = context;
        _currentUser = currentUser;
        _logger = logger;
        _validator = validator;
    }
    
    public async Task<Result<HotelResponse>> Handle(GetHotelQuery request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for GetHotelQuery: {Errors}", validationResult.Errors);
            return Result.Fail(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
        }
        
        var managerId = _currentUser.GetUserId();
        var hotel = await _context.Hotels
            .Where(h => h.Id == request.HotelId && h.OwnerId == managerId)
            .ProjectToType<HotelResponse>()
            .FirstOrDefaultAsync(cancellationToken);

        if (hotel == null)
        {
            _logger.LogWarning("Hotel with ID {HotelId} not found for manager with ID {ManagerId}", request.HotelId, managerId);
            return Result.Fail(new Error("Hotel not found or access denied."));
        }

        return Result.Ok(hotel);
    }
}