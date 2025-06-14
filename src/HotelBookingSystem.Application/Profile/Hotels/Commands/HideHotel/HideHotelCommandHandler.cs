using FluentResults;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using HotelBookingSystem.Application.Common.Interfaces.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Profile.Commands.HideHotel;

public class HideHotelCommandHandler : IRequestHandler<HideHotelCommand, Result>
{
    private readonly IHotelBookingDbContext _context;
    private readonly ICurrentUserService _currentUser;
    
    public HideHotelCommandHandler(IHotelBookingDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }
    
    public async Task<Result> Handle(HideHotelCommand request, CancellationToken cancellationToken)
    {
        var managerId = _currentUser.GetUserId();
        var hotel = await _context.Hotels
            .FirstOrDefaultAsync(h => h.Id == request.HotelId && h.OwnerId == managerId, cancellationToken);

        if (hotel == null)
            return Result.Fail(new Error("Hotel not found or access denied."));
        
        if (!hotel.IsVisible)
            return Result.Fail(new Error("Hotel is already hidden."));
        
        hotel.IsVisible = false;
        hotel.UpdatedAt = DateTime.UtcNow;
        
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Ok();
    }
}