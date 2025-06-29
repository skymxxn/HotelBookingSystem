using HotelBookingSystem.Application.Features.Moderation.Roles.Queries.GetRoles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.API.Controllers.Moderation;

[ApiController]
[Route("api/moderation/roles")]
[Authorize(Roles = "Moderator, Admin")]
public class RoleModerationController : ControllerBase
{
    private readonly ISender _mediator;

    public RoleModerationController(ISender mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetRoles()
    {
        var result = await _mediator.Send(new GetRolesQuery());
        return Ok(result.Value);
    }
}