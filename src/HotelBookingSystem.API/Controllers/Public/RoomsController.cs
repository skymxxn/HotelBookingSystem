using HotelBookingSystem.Application.Features.Public.Rooms.Queries.GetRoom;
using HotelBookingSystem.Application.Features.Public.Rooms.Queries.GetRooms;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.API.Controllers.Public;

[ApiController]
[Route("api/hotels/{hotelId:guid}/rooms")]
public class RoomsController : ControllerBase
{
    private readonly ISender _mediator;

    public RoomsController(ISender mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetRooms(Guid hotelId)
    {
        var query = new GetRoomsQuery(hotelId);
        var result = await _mediator.Send(query);

        if (result.IsFailed)
            return NotFound(result.Errors);

        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetRoom(Guid hotelId, Guid id)
    {
        var query = new GetRoomQuery(hotelId, id);
        var result = await _mediator.Send(query);

        if (result.IsFailed)
            return NotFound(result.Errors);

        return Ok(result.Value);
    }
}