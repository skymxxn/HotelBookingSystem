using HotelBookingSystem.Application.Common.DTOs;
using HotelBookingSystem.Application.Common.Interfaces.Users;
using HotelBookingSystem.Application.Hotels.ApproveHotel;
using HotelBookingSystem.Application.Hotels.CreateHotel;
using HotelBookingSystem.Application.Hotels.DeleteHotel;
using HotelBookingSystem.Application.Hotels.GetHotels;
using HotelBookingSystem.Application.Hotels.GetMyHotelById;
using HotelBookingSystem.Application.Hotels.GetMyHotels;
using HotelBookingSystem.Application.Hotels.GetPendingHotelById;
using HotelBookingSystem.Application.Hotels.GetPendingHotels;
using HotelBookingSystem.Application.Hotels.UpdateHotel;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HotelsController : ControllerBase
{
    private readonly ISender _mediator;
    private readonly ICurrentUserService _currentUser;

    public HotelsController(ISender mediator, ICurrentUserService currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    [HttpGet]
    public async Task<IActionResult> GetApprovedHotels()
    {
        var result = await _mediator.Send(new GetHotelsQuery());
        return Ok(result);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetHotelById(Guid id)
    {
        var query = new GetPendingHotelByIdQuery(id);
        var result = await _mediator.Send(query);
        
        if (result == null)
            return NotFound();
        
        return Ok(result);
    }
    
    [Authorize(Roles = "Manager")]
    [HttpGet("my")]
    public async Task<IActionResult> GetMyHotels()
    {
        var managerId = _currentUser.GetUserId();
        var query = new GetMyHotelsQuery(managerId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [Authorize(Roles =  "Manager")]
    [HttpGet("my/{id:guid}")]
    public async Task<IActionResult> GetMyHotelById(Guid id)
    {
        var managerId = _currentUser.GetUserId();
        var query = new GetMyHotelByIdQuery(id, managerId);
        var result = await _mediator.Send(query);
        
        if (result == null)
            return NotFound();
        
        return Ok(result);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet("pending")]
    public async Task<IActionResult> GetPendingHotels()
    {
        var result = await _mediator.Send(new GetPendingHotelsQuery());
        return Ok(result);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet("pending/{id:guid}")]
    public async Task<IActionResult> GetPendingHotelById(Guid id)
    {
        var query = new GetPendingHotelByIdQuery(id);
        var result = await _mediator.Send(query);
        
        if (result == null)
            return NotFound();
        
        return Ok(result);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost("approve/{id:guid}")]
    public async Task<IActionResult> ApproveHotel(Guid id)
    {
        var command = new ApproveHotelCommand(id);
        var result = await _mediator.Send(command);
        
        if (result.IsFailed)
            return BadRequest(result.Errors);
        
        return NoContent();
    }
    
    [Authorize(Roles = "Manager")]
    [HttpPost]
    public async Task<IActionResult> CreateHotel([FromBody] HotelRequest request)
    {
        var command = request.Adapt<CreateHotelCommand>();
        var result = await _mediator.Send(command);
        
        if (result.IsFailed)
            return BadRequest(result.Errors);
        
        return CreatedAtAction(nameof(CreateHotel), new { id = result.Value }, result.Value);
    }
    
    [Authorize(Roles = "Manager")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateHotel(Guid id, [FromBody] HotelRequest request)
    {
        var command = request.Adapt<UpdateHotelCommand>() with { Id = id };
        var result = await _mediator.Send(command);
        
        if (result.IsFailed)
            return BadRequest(result.Errors);
        
        return NoContent();
    }
    
    [Authorize(Roles = "Manager")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteHotel(Guid id)
    {
        var command = new DeleteHotelCommand(id);
        var result = await _mediator.Send(command);
        
        if (result.IsFailed)
            return BadRequest(result.Errors);
        
        return NoContent();
    }
}