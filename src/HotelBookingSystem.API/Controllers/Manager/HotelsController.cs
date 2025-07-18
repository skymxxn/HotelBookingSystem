﻿using HotelBookingSystem.Application.Common.DTOs.Common;
using HotelBookingSystem.Application.Common.DTOs.Hotels;
using HotelBookingSystem.Application.Features.Manager.Hotels.Commands.CreateHotel;
using HotelBookingSystem.Application.Features.Manager.Hotels.Commands.DeleteHotel;
using HotelBookingSystem.Application.Features.Manager.Hotels.Commands.SetHotelPublication;
using HotelBookingSystem.Application.Features.Manager.Hotels.Commands.UpdateHotel;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.API.Controllers.Manager;

[ApiController]
[Authorize(Roles = "Manager, Admin")]
[Route("api/manager/hotels")]
public class HotelsController : ControllerBase
{
    private readonly ISender _mediator;
    
    public HotelsController(ISender mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPatch("{id:guid}/publication")]
    public async Task<IActionResult> SetHotelPublication(Guid id, [FromBody] SetPublicationRequest request)
    {
        var command = new SetHotelPublicationCommand(id, request.IsPublished);
        var result = await _mediator.Send(command);
        
        if (result.IsFailed)
            return BadRequest(result.Errors);
        
        return NoContent();
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateHotel([FromBody] HotelRequest request)
    {
        var command = request.Adapt<CreateHotelCommand>();
        var result = await _mediator.Send(command);
        
        if (result.IsFailed)
            return BadRequest(result.Errors);
        
        return CreatedAtAction(nameof(CreateHotel), new { id = result.Value }, result.Value);
    }
    
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateHotel(Guid id, [FromBody] HotelRequest request)
    {
        var command = request.Adapt<UpdateHotelCommand>() with { Id = id };
        var result = await _mediator.Send(command);
        
        if (result.IsFailed)
            return BadRequest(result.Errors);
        
        return NoContent();
    }
    
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