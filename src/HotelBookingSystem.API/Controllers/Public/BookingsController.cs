using HotelBookingSystem.Application.Features.Bookings.Commands.CancelBooking;
using HotelBookingSystem.Application.Features.Bookings.Commands.ConfirmBooking;
using HotelBookingSystem.Application.Features.Bookings.Commands.CreateBooking;
using HotelBookingSystem.Application.Features.Bookings.Queries.GetBooking;
using HotelBookingSystem.Application.Features.Bookings.Queries.GetBookings;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.API.Controllers.Public;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BookingsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetBookings([FromQuery] GetBookingsQuery query)
    {
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
    
    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingCommand command)
    {
        var result = await _mediator.Send(command);
        
        if (result.IsFailed)
            return BadRequest(result.Errors);
        
        return CreatedAtAction(nameof(GetBooking), new { id = result.Value.Id }, result.Value);
    }

    [HttpPost("confirm-booking")]
    public async Task<IActionResult> ConfirmBooking([FromQuery] ConfirmBookingCommand command)
    {
        var result = await _mediator.Send(command);
        
        if (result.IsFailed)
            return BadRequest(result.Errors);
        
        return Ok("Booking confirmed.");
    }
    
    [HttpPost("{id:guid}/cancel")]
    public async Task<IActionResult> CancelBooking(Guid id, [FromBody] CancelBookingCommand request)
    {
        var command = request with { BookingId = id };
        var result = await _mediator.Send(command);
        
        if (result.IsFailed)
            return BadRequest(result.Errors);
        
        return Ok(result.Value);
    }
}