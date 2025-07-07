using HotelBookingSystem.Application.Features.Manager.Bookings.Commands.ConfirmBooking;
using HotelBookingSystem.Application.Features.Manager.Bookings.Commands.RejectBooking;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.API.Controllers.Manager;

[ApiController]
[Authorize(Roles = "Manager, Admin")]
[Route("api/manager/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BookingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("{id:guid}/confirm")]
    public async Task<IActionResult> ConfirmBooking(Guid id)
    {
        var command = new ConfirmBookingCommand(id);
        var result = await _mediator.Send(command);
        
        if (result.IsFailed)
            return BadRequest(result.Errors);
        
        return Ok(result.Value);
    }

    [HttpPost("{id:guid}/reject")]
    public async Task<IActionResult> RejectBooking(Guid id, [FromBody] RejectBookingCommand request)
    {
        var command = request with { BookingId = id };
        var result = await _mediator.Send(command);
        
        if (result.IsFailed)
            return BadRequest(result.Errors);
        
        return Ok(result.Value);
    }
}