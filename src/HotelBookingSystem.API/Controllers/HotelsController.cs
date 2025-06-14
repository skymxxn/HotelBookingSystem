using HotelBookingSystem.Application.Admin.Queries.GetPendingHotelById;
using HotelBookingSystem.Application.Hotels.Queries.GetHotels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.API.Controllers;

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
}