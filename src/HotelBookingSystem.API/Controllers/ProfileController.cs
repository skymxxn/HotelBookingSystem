using HotelBookingSystem.Application.Common.DTOs;
using HotelBookingSystem.Application.Profile.Commands.CreateHotel;
using HotelBookingSystem.Application.Profile.Commands.DeleteHotel;
using HotelBookingSystem.Application.Profile.Commands.HideHotel;
using HotelBookingSystem.Application.Profile.Commands.PublishHotel;
using HotelBookingSystem.Application.Profile.Commands.UpdateHotel;
using HotelBookingSystem.Application.Profile.Queries.GetMyHotelById;
using HotelBookingSystem.Application.Profile.Queries.GetMyHotels;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfileController : ControllerBase
{
    private readonly ISender _mediator;
    
    public ProfileController(ISender mediator)
    {
        _mediator = mediator;
    }
    
    [Authorize(Roles = "Manager")]
    [HttpGet("hotels")]
    public async Task<IActionResult> GetMyHotels()
    {
        var query = new GetMyHotelsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [Authorize(Roles = "Manager")]
    [HttpGet("hotels/{id:guid}")]
    public async Task<IActionResult> GetMyHotelById(Guid id)
    {
        var query = new GetMyHotelByIdQuery(id);
        var result = await _mediator.Send(query);
        
        if (result == null)
            return NotFound();
        
        return Ok(result);
    }
    
    [Authorize(Roles = "Manager")]
    [HttpPost("hotels/{id:guid}/publish")]
    public async Task<IActionResult> PublishHotel(Guid id)
    {
        var command = new PublishHotelCommand(id);
        var result = await _mediator.Send(command);
        
        if (result.IsFailed)
            return BadRequest(result.Errors);
        
        return NoContent();
    }
    
    [Authorize(Roles = "Manager")]
    [HttpPost("hotels/{id:guid}/hide")]
    public async Task<IActionResult> HideHotel(Guid id)
    {
        var command = new HideHotelCommand(id);
        var result = await _mediator.Send(command);
        
        if (result.IsFailed)
            return BadRequest(result.Errors);
        
        return NoContent();
    }
    
    [Authorize(Roles = "Manager")]
    [HttpPost("hotels")]
    public async Task<IActionResult> CreateHotel([FromBody] HotelRequest request)
    {
        var command = request.Adapt<CreateHotelCommand>();
        var result = await _mediator.Send(command);
        
        if (result.IsFailed)
            return BadRequest(result.Errors);
        
        return CreatedAtAction(nameof(CreateHotel), new { id = result.Value }, result.Value);
    }
    
    [Authorize(Roles = "Manager")]
    [HttpPut("hotels/{id:guid}")]
    public async Task<IActionResult> UpdateHotel(Guid id, [FromBody] HotelRequest request)
    {
        var command = request.Adapt<UpdateHotelCommand>() with { Id = id };
        var result = await _mediator.Send(command);
        
        if (result.IsFailed)
            return BadRequest(result.Errors);
        
        return NoContent();
    }
    
    [Authorize(Roles = "Manager")]
    [HttpDelete("hotels/{id:guid}")]
    public async Task<IActionResult> DeleteHotel(Guid id)
    {
        var command = new DeleteHotelCommand(id);
        var result = await _mediator.Send(command);
        
        if (result.IsFailed)
            return BadRequest(result.Errors);
        
        return NoContent();
    }
}