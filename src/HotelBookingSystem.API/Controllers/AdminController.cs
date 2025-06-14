using HotelBookingSystem.Application.Admin.Commands.ApproveHotel;
using HotelBookingSystem.Application.Admin.Queries.GetPendingHotelById;
using HotelBookingSystem.Application.Admin.Queries.GetPendingHotels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly ISender _mediator;
    
    public AdminController(ISender mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("hotels/pending")]
    public async Task<IActionResult> GetPendingHotels()
    {
        var result = await _mediator.Send(new GetPendingHotelsQuery());
        return Ok(result);
    }
    
    [HttpGet("hotels/pending/{id:guid}")]
    public async Task<IActionResult> GetPendingHotelById(Guid id)
    {
        var query = new GetPendingHotelByIdQuery(id);
        var result = await _mediator.Send(query);
        
        if (result == null)
            return NotFound();
        
        return Ok(result);
    }
    
    [HttpPost("hotels/{id:guid}/approve")]
    public async Task<IActionResult> ApproveHotel(Guid id)
    {
        var command = new ApproveHotelCommand(id);
        var result = await _mediator.Send(command);
        
        if (result.IsFailed)
            return BadRequest(result.Errors);
        
        return NoContent();
    }
}