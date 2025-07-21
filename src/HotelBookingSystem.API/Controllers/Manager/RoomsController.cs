using HotelBookingSystem.Application.Common.DTOs.Common;
using HotelBookingSystem.Application.Common.DTOs.Rooms;
using HotelBookingSystem.Application.Features.Manager.Rooms.Commands.CreateRoom;
using HotelBookingSystem.Application.Features.Manager.Rooms.Commands.DeleteRoom;
using HotelBookingSystem.Application.Features.Manager.Rooms.Commands.SetRoomPublication;
using HotelBookingSystem.Application.Features.Manager.Rooms.Commands.UpdateRoom;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.API.Controllers.Manager;

[ApiController]
[Authorize(Roles = "Manager, Admin")]
[Route("api/manager/rooms")]
public class RoomsController : ControllerBase
{
    private readonly ISender _mediator;
    
    public RoomsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRoom([FromBody] CreateRoomRequest request)
    {
        var command = request.Adapt<CreateRoomCommand>();
        var result = await _mediator.Send(command);

        if (result.IsFailed)
            return BadRequest(result.Errors);

        return  Ok(result.Value);
    }

    [HttpPatch("{id:guid}/publication")]
    public async Task<IActionResult> SetRoomPublication(Guid id, [FromBody] SetPublicationRequest request)
    {
        var command = new SetRoomPublicationCommand(id, request.IsPublished);
        var result = await _mediator.Send(command);

        if (result.IsFailed)
            return BadRequest(result.Errors);

        return NoContent();
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateRoom(Guid id, [FromBody] RoomRequest request)
    {
        var command = request.Adapt<UpdateRoomCommand>() with {RoomId = id};
        var result = await _mediator.Send(command);

        if (result.IsFailed)
            return BadRequest(result.Errors);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteRoom(Guid id)
    {
        var command = new DeleteRoomCommand(id);
        var result = await _mediator.Send(command);

        if (result.IsFailed)
            return BadRequest(result.Errors);

        return NoContent();
    }
}
