using FluentResults;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using HotelBookingSystem.Application.Common.Interfaces.Users;
using HotelBookingSystem.Domain.Entities;
using MediatR;

namespace HotelBookingSystem.Application.Features.Profile.Hotels.Commands.CreateHotel;

public class CreateHotelCommandHandler : IRequestHandler<CreateHotelCommand, Result<Guid>>
{
    private readonly IHotelBookingDbContext _context;
    private readonly ICurrentUserService _currentUser;
    
    public CreateHotelCommandHandler(IHotelBookingDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
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
        
        return Result.Ok(hotel.Id);
    }
}