using FluentResults;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using HotelBookingSystem.Application.Common.Interfaces.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Profile.Commands.DeleteHotel;

public class DeleteHotelCommandHandler : IRequestHandler<DeleteHotelCommand, Result>
{
    private readonly IHotelBookingDbContext _context;
    private readonly ICurrentUserService _currentUser;
    
    public DeleteHotelCommandHandler(IHotelBookingDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }
    
    public async Task<Result> Handle(DeleteHotelCommand request, CancellationToken cancellationToken)
    {
        var hotel = await _context.Hotels
            .FirstOrDefaultAsync(h => h.Id == request.Id, cancellationToken);
        
        if (hotel == null)
        {
            return Result.Fail("Hotel not found.");
        }
        
        if (hotel.OwnerId != _currentUser.GetUserId())
        {
            return Result.Fail("You do not have permission to delete this hotel.");
        }
        
        _context.Hotels.Remove(hotel);
        await _context.SaveChangesAsync(cancellationToken);
        
        return Result.Ok();
    }
}