using HotelBookingSystem.Application.Features.Moderation.Hotels.Commands.ApproveHotel;
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