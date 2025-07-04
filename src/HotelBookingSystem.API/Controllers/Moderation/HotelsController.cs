using HotelBookingSystem.Application.Features.Moderation.Hotels.Commands.ApproveHotel;
using HotelBookingSystem.Application.Features.Moderation.Hotels.Queries.GetPendingHotel;
using HotelBookingSystem.Application.Features.Moderation.Hotels.Queries.GetPendingHotels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.API.Controllers.Moderation;

[ApiController]
[Route("api/moderation/hotels")]
[Authorize(Roles = "Moderator, Admin")]
public class HotelsController : ControllerBase
{
    private readonly ISender _mediator;
    
    public HotelsController(ISender mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("pending")]
    public async Task<IActionResult> GetPendingHotels()
    {
        var result = await _mediator.Send(new GetPendingHotelsQuery());
        return Ok(result.Value);
    }
    
    [HttpGet("pending/{id:guid}")]
    public async Task<IActionResult> GetPendingHotel(Guid id)
    {
        var query = new GetPendingHotelQuery(id);
        var result = await _mediator.Send(query);
        
        if (result.IsFailed)
            return NotFound(result.Errors);
        
        return Ok(result.Value);
    }
    
    [HttpPost("{id:guid}/approve")]
    public async Task<IActionResult> ApproveHotel(Guid id)
    {
        var command = new ApproveHotelCommand(id);
        var result = await _mediator.Send(command);
        
        if (result.IsFailed)
            return BadRequest(result.Errors);
        
        return NoContent();
    }
}