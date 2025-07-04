using HotelBookingSystem.Application.Features.Moderation.Users.Commands.AssignRoleToUser;
using HotelBookingSystem.Application.Features.Moderation.Users.Commands.RemoveRoleFromUser;
using HotelBookingSystem.Application.Features.Moderation.Users.DTOs;
using HotelBookingSystem.Application.Features.Moderation.Users.Queries.GetUser;
using HotelBookingSystem.Application.Features.Moderation.Users.Queries.GetUsers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.API.Controllers.Moderation;

[ApiController]
[Route("api/moderation/users")]
[Authorize(Roles = "Moderator, Admin")]
public class UsersController : ControllerBase
{
    private readonly ISender _mediator;

    public UsersController(ISender mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var result = await _mediator.Send(new GetUsersQuery());
        return Ok(result.Value);
    }

    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetUser(Guid userId)
    {
        var query = new GetUserQuery(userId);
        var result = await _mediator.Send(query);
        
        if (result.IsFailed)
            return NotFound(result.Errors);
        
        return Ok(result.Value);
    }
    
    [HttpPost("{userId:guid}/assign-role")]
    public async Task<IActionResult> AssignRoleToUser(Guid userId, [FromBody] RoleRequest request)
    {
        var command = new AssignRoleToUserCommand(userId, request.RoleId);
        var result = await _mediator.Send(command);
        
        if (result.IsFailed)
            return BadRequest(result.Errors);
        
        return NoContent();
    }
    
    [HttpPost("{userId:guid}/remove-role")]
    public async Task<IActionResult> RemoveRoleFromUser(Guid userId, [FromBody] RoleRequest request)
    {
        var command = new RemoveRoleFromUserCommand(userId, request.RoleId);
        var result = await _mediator.Send(command);
        
        if (result.IsFailed)
            return BadRequest(result.Errors);
        
        return NoContent();
    }
}