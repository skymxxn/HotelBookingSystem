using FluentResults;
using FluentValidation;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using HotelBookingSystem.Application.Common.Interfaces.Users;
using HotelBookingSystem.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Application.Features.Manager.Hotels.Commands.CreateHotel;

public class CreateHotelCommandHandler : IRequestHandler<CreateHotelCommand, Result<Guid>>
{
    private readonly IHotelBookingDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<CreateHotelCommand> _validator;
    private readonly ILogger<CreateHotelCommandHandler> _logger;
    
    public CreateHotelCommandHandler(IHotelBookingDbContext context, ICurrentUserService currentUser, ILogger<CreateHotelCommandHandler> logger, IValidator<CreateHotelCommand> validator)
    {
        _context = context;
        _currentUser = currentUser;
        _logger = logger;
        _validator = validator;
    }
    
    public async Task<Result<Guid>> Handle(CreateHotelCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for CreateHotelCommand: {Errors}", validationResult.Errors);
            return Result.Fail(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
        }
        
        var hotel = new Hotel
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Address = request.Address,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            OwnerId = _currentUser.GetUserId()
        };
        
        _context.Hotels.Add(hotel);
        await _context.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Hotel with ID {HotelId} created by user {UserId} with roles: {Roles}", hotel.Id, _currentUser.GetUserId(), _currentUser.GetRoles());
        
        return Result.Ok(hotel.Id);
    }
}