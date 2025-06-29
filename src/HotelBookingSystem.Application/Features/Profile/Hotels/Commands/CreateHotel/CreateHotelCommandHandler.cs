using FluentResults;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using HotelBookingSystem.Application.Common.Interfaces.Users;
using HotelBookingSystem.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Application.Features.Profile.Hotels.Commands.CreateHotel;

public class CreateHotelCommandHandler : IRequestHandler<CreateHotelCommand, Result<Guid>>
{
    private readonly IHotelBookingDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly ILogger<CreateHotelCommandHandler> _logger;
    
    public CreateHotelCommandHandler(IHotelBookingDbContext context, ICurrentUserService currentUser, ILogger<CreateHotelCommandHandler> logger)
    {
        _context = context;
        _currentUser = currentUser;
        _logger = logger;
    }
    
    public async Task<Result<Guid>> Handle(CreateHotelCommand request, CancellationToken cancellationToken)
    {
        var hotel = new Hotel
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Address = request.Address,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsApproved = false,
            IsVisible = false,
            OwnerId = _currentUser.GetUserId()
        };
        
        _context.Hotels.Add(hotel);
        await _context.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Hotel with ID {HotelId} created by user {UserId}", hotel.Id, _currentUser.GetUserId());
        
        return Result.Ok(hotel.Id);
    }
}