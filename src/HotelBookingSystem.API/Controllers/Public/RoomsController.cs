using HotelBookingSystem.Application.Features.Rooms.Queries.GetRoom;
using HotelBookingSystem.Application.Features.Rooms.Queries.GetRooms;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.API.Controllers.Public;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly ISender _mediator;

    public RoomsController(ISender mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetRooms([FromQuery]  GetRoomsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetRoom(Guid id)
    {
        var query = new GetRoomQuery(id);
        var result = await _mediator.Send(query);

        if (result.IsFailed)
            return NotFound(result.Errors);

        return Ok(result.Value);
    }
}