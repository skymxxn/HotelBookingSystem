using FluentResults;
using HotelBookingSystem.Application.Common.Interfaces.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.Admin.Commands.ApproveHotel;

public class ApproveHotelCommandHandler : IRequestHandler<ApproveHotelCommand, Result>
{
    private readonly IHotelBookingDbContext _context;
    
    public ApproveHotelCommandHandler(IHotelBookingDbContext context)
    {
        _context = context;
    }
    
    public async Task<Result> Handle(ApproveHotelCommand request, CancellationToken cancellationToken)
    {
        var hotel = await _context.Hotels
            .FirstOrDefaultAsync(h => h.Id == request.HotelId, cancellationToken);
        
        if (hotel == null) 
            return Result.Fail("Hotel not found.");
        
        if (hotel.IsApproved) 
            return Result.Fail("Hotel is already approved.");
        
        hotel.IsApproved = true;
        await _context.SaveChangesAsync(cancellationToken);
        
        return Result.Ok();
    }
}