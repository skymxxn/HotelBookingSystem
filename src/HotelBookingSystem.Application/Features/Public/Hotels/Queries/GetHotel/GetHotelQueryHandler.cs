using FluentResults;
using FluentValidation;
using HotelBookingSystem.Application.Common.DTOs;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Application.Features.Public.Hotels.Queries.GetHotel;

public class GetHotelQueryHandler : IRequestHandler<GetHotelQuery, Result<HotelResponse>>
{
    private readonly IHotelBookingDbContext _context;
    private readonly IValidator<GetHotelQuery> _validator;
    private readonly ILogger<GetHotelQueryHandler> _logger;
    
    public GetHotelQueryHandler(IHotelBookingDbContext context, ILogger<GetHotelQueryHandler> logger, IValidator<GetHotelQuery> validator)
    {
        _context = context;
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
        
        var hotel = await _context.Hotels
            .Where(h => h.Id == request.HotelId && h.IsPublished)
            .ProjectToType<HotelResponse>()
            .FirstOrDefaultAsync(cancellationToken);
        
        if (hotel == null)
        {
            _logger.LogWarning("Hotel with ID {HotelId} not found or not published", request.HotelId);
            return Result.Fail(new Error("Hotel not found or not published."));
        }
        
        return hotel;
    }
}