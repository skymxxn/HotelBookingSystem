using FluentResults;
using FluentValidation;
using HotelBookingSystem.Application.Common.DTOs.Hotels;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using HotelBookingSystem.Application.Common.Interfaces.Users;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Application.Features.Hotels.Queries.GetHotel;

public class GetHotelQueryHandler : IRequestHandler<GetHotelQuery, Result<HotelResponse>>
{
    private readonly IHotelBookingDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<GetHotelQuery> _validator;
    private readonly ILogger<GetHotelQueryHandler> _logger;
    
    public GetHotelQueryHandler(IHotelBookingDbContext context, ILogger<GetHotelQueryHandler> logger, IValidator<GetHotelQuery> validator, ICurrentUserService currentUser)
    {
        _context = context;
        _logger = logger;
        _validator = validator;
        _currentUser = currentUser;
    }

    public async Task<Result<HotelResponse>> Handle(GetHotelQuery request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for GetHotelQuery: {Errors}", validationResult.Errors);
            return Result.Fail(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
        }

        var query = _context.Hotels
            .Where(h => h.Id == request.HotelId);
        
        if (_currentUser.IsUser())
        {
            query = query.Where(h => h.IsPublished && h.IsApproved);
        }
        else if (_currentUser.IsManager())
        {
            var userId = _currentUser.GetUserId();
            query = query.Where(h => h.OwnerId == userId);
        }
        
        var hotel = await query
            .ProjectToType<HotelResponse>()
            .FirstOrDefaultAsync(cancellationToken);
        
        if (hotel == null)
        {
            _logger.LogWarning("Hotel with ID {HotelId} not found or access denied", request.HotelId);
            return Result.Fail(new Error("Hotel not found or access denied."));
        }
        
        return hotel;
    }
}