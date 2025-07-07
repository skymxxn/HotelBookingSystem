using HotelBookingSystem.Application.Features.Manager.Bookings.Commands.ConfirmBooking;
using HotelBookingSystem.Application.Features.Manager.Bookings.Commands.RejectBooking;
using HotelBookingSystem.Application.Features.Manager.Bookings.Queries.GetBooking;
using HotelBookingSystem.Application.Features.Manager.Bookings.Queries.GetBookings;
using HotelBookingSystem.Domain.Enums;
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
    
    [HttpGet]
    public async Task<IActionResult> GetBookings(
        [FromQuery] Guid? roomId,
        [FromQuery] Guid? userId,
        [FromQuery] BookingStatus? status,
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate
        )
    {
        var query = new GetBookingsQuery
        {
            RoomId = roomId,
            UserId = userId,
            Status = status,
            FromDate = fromDate,
            ToDate = toDate
        };
        var result = await _mediator.Send(query);
        
        return Ok(result.Value);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetBooking(Guid id)
    {
        var query = new GetBookingQuery(id);
        var result = await _mediator.Send(query);
        
        if (result.IsFailed)
            return NotFound(result.Errors);
        
        return Ok(result.Value);
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