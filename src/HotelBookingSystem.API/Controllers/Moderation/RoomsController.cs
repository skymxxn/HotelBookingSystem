using HotelBookingSystem.Application.Features.Moderation.Rooms.Commands.ApproveRoom;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.API.Controllers.Moderation;

[ApiController]
[Route("api/moderation/rooms")]
[Authorize(Roles = "Moderator, Admin")]
public class RoomsController : ControllerBase
{
    private readonly ISender _mediator;

    public RoomsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("{id:guid}/approve")]
    public async Task<IActionResult> ApproveRoom(Guid id)
    {
        var command = new ApproveRoomCommand(id);
        var result = await _mediator.Send(command);

        if (result.IsFailed)
            return BadRequest(result.Errors);

        return NoContent();
    }
}