using HotelBookingSystem.Application.Common.DTOs;
using HotelBookingSystem.Application.Features.Manager.Rooms.Commands.CreateRoom;
using HotelBookingSystem.Application.Features.Manager.Rooms.Commands.DeleteRoom;
using HotelBookingSystem.Application.Features.Manager.Rooms.Commands.SetRoomPublication;
using HotelBookingSystem.Application.Features.Manager.Rooms.Commands.UpdateRoom;
using HotelBookingSystem.Application.Features.Manager.Rooms.Queries.GetRoom;
using HotelBookingSystem.Application.Features.Manager.Rooms.Queries.GetRooms;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.API.Controllers.Manager;

[ApiController]
[Authorize(Roles = "Manager, Admin")]
[Route("api/manager/hotels/{hotelId:guid}/rooms")]
public class RoomsController : ControllerBase
{
    private readonly ISender _mediator;
    
    public RoomsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetMyRooms(Guid hotelId)
    {
        var query = new GetRoomsQuery(hotelId);
        var result = await _mediator.Send(query);

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

    [HttpPost]
    public async Task<IActionResult> CreateRoom(Guid hotelId, [FromBody] RoomRequest request)
    {
        var command = request.Adapt<CreateRoomCommand>() with { HotelId = hotelId };
        var result = await _mediator.Send(command);

        if (result.IsFailed)
            return BadRequest(result.Errors);

        return CreatedAtAction(nameof(GetRoom), new { hotelId, id = result.Value }, result.Value);
    }

    [HttpPatch("{id:guid}/publication")]
    public async Task<IActionResult> SetRoomPublication(Guid hotelId, Guid id, [FromBody] SetPublicationRequest request)
    {
        var command = new SetRoomPublicationCommand(hotelId, id, request.IsPublished);
        var result = await _mediator.Send(command);

        if (result.IsFailed)
            return BadRequest(result.Errors);

        return NoContent();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateRoom(Guid hotelId, Guid id, [FromBody] RoomRequest request)
    {
        var command = request.Adapt<UpdateRoomCommand>() with { HotelId = hotelId, RoomId = id };
        var result = await _mediator.Send(command);

        if (result.IsFailed)
            return BadRequest(result.Errors);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteRoom(Guid hotelId, Guid id)
    {
        var command = new DeleteRoomCommand(hotelId, id);
        var result = await _mediator.Send(command);

        if (result.IsFailed)
            return BadRequest(result.Errors);

        return NoContent();
    }
}
