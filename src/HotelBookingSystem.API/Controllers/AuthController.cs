using HotelBookingSystem.Application.Features.Authentication.Commands.Login;
using HotelBookingSystem.Application.Features.Authentication.Commands.Logout;
using HotelBookingSystem.Application.Features.Authentication.Commands.RefreshToken;
using HotelBookingSystem.Application.Features.Authentication.Commands.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace HotelBookingSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("strict")]
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
            return BadRequest(result.Errors);
        
        return Ok(result.Value);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await _mediator.Send(command);
        
        if (result.IsFailed)
            return BadRequest(result.Errors);
        
        return Ok(result.Value);
    }
    
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
    {
        var result = await _mediator.Send(command);
        
        if (result.IsFailed)
            return BadRequest(result.Errors);
        
        return Ok(result.Value);
    }
    
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutCommand command)
    {
        var result = await _mediator.Send(command);
        
        if (result.IsFailed)
            return BadRequest(result.Errors);
        
        return Ok();
    }
}