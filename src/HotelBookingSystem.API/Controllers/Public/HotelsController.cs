﻿using HotelBookingSystem.Application.Features.Hotels.Queries.GetHotel;
using HotelBookingSystem.Application.Features.Hotels.Queries.GetHotels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.API.Controllers.Public;

[ApiController]
[Route("api/[controller]")]
public class HotelsController : ControllerBase
{
    private readonly ISender _mediator;

    public HotelsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetHotels([FromQuery] GetHotelsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result.Value);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetHotel(Guid id)
    {
        var query = new GetHotelQuery(id);
        var result = await _mediator.Send(query);
        
        if (result.IsFailed)
            return NotFound(result.Errors);
        
        return Ok(result.Value);
    }
}