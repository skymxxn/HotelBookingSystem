using FluentResults;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using HotelBookingSystem.Application.Common.Interfaces.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.Profile.Hotels.Commands.UpdateHotel;

public class UpdateHotelCommandHandler : IRequestHandler<UpdateHotelCommand, Result>
{
    private readonly IHotelBookingDbContext _context;
    private readonly ICurrentUserService _currentUser;
    
    public UpdateHotelCommandHandler(IHotelBookingDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(UpdateHotelCommand request, CancellationToken cancellationToken)
    {
        var hotel = await _context.Hotels
            .FirstOrDefaultAsync(h => h.Id == request.Id, cancellationToken);
        
        if (hotel == null)
        {
            return Result.Fail("Hotel not found.");
        }
        
        if (hotel.OwnerId != _currentUser.GetUserId())
        {
            return Result.Fail("You do not have permission to update this hotel.");
        }
        
        hotel.Name = request.Name;
        hotel.Description = request.Description;
        hotel.Address = request.Address;
        hotel.UpdatedAt = DateTime.UtcNow;
        hotel.IsApproved = false;
        hotel.IsVisible = false;
        
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Ok();
    }
}