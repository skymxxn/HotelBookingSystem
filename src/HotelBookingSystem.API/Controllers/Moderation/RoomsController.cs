using HotelBookingSystem.Application.Features.Moderation.Rooms.Commands.ApproveRoom;
using HotelBookingSystem.Application.Features.Moderation.Rooms.Queries.GetPendingRoom;
using HotelBookingSystem.Application.Features.Moderation.Rooms.Queries.GetPendingRooms;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.API.Controllers.Moderation;

[ApiController]
[Route("api/moderation/hotels/{hotelId:guid}/rooms")]
[Authorize(Roles = "Moderator, Admin")]
public class RoomsController : ControllerBase
{
    private readonly ISender _mediator;

    public RoomsController(ISender mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("pending")]
    public async Task<IActionResult> GetPendingRooms(Guid hotelId)
    {
        var query = new GetPendingRoomsQuery(hotelId);
        var result = await _mediator.Send(query);

        return Ok(result.Value);
    }
    
    [HttpGet("pending/{id:guid}")]
    public async Task<IActionResult> GetPendingRoom(Guid hotelId, Guid id)
    {
        var query = new GetPendingRoomQuery(hotelId, id);
        var result = await _mediator.Send(query);

        if (result.IsFailed)
            return NotFound(result.Errors);

        return Ok(result.Value);
    }

    [HttpPost("{id:guid}/approve")]
    public async Task<IActionResult> ApproveRoom(Guid hotelId, Guid id)
    {
        var command = new ApproveRoomCommand(hotelId, id);
        var result = await _mediator.Send(command);

        if (result.IsFailed)
            return BadRequest(result.Errors);

        return NoContent();
    }
}