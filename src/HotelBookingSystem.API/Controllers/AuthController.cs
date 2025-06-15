using HotelBookingSystem.Application.Features.Authentication.Login;
using HotelBookingSystem.Application.Features.Authentication.Logout;
using HotelBookingSystem.Application.Features.Authentication.RefreshToken;
using HotelBookingSystem.Application.Features.Authentication.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ISender _mediator;

    public AuthController(ISender mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.IsFailed)
        {
            return BadRequest(result.Errors);
        }
        
        return Ok(result.Value);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.IsFailed)
        {
            return BadRequest(result.Errors);
        }
        
        return Ok(result.Value);
    }
    
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.IsFailed)
        {
            return BadRequest(result.Errors);
        }
        
        return Ok(result.Value);
    }
    
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.IsFailed)
        {
            return BadRequest(result.Errors);
        }
        
        return Ok();
    }
}