using HotelBookingSystem.Application.Features.Authentication.Commands.ForgotPassword;
using HotelBookingSystem.Application.Features.Authentication.Commands.Login;
using HotelBookingSystem.Application.Features.Authentication.Commands.Logout;
using HotelBookingSystem.Application.Features.Authentication.Commands.RefreshToken;
using HotelBookingSystem.Application.Features.Authentication.Commands.Register;
using HotelBookingSystem.Application.Features.Authentication.Commands.ResetPassword;
using HotelBookingSystem.Application.Features.Authentication.Commands.VerifyEmail;
using HotelBookingSystem.Application.Features.Authentication.DTOs;
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
        
        return Ok("User successfully registered. Email verification sent.");
    }
    
    [HttpGet("verify-email")]
    public async Task<IActionResult> VerifyEmail([FromQuery] string token)
    {
        var result = await _mediator.Send(new VerifyEmailCommand(token));
        
        if (result.IsFailed)
            return BadRequest(result.Errors);

        return Ok("Email successfully verified.");
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

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
    {
        var result = await _mediator.Send(command);
        
        if (result.IsFailed)
            return BadRequest(result.Errors);
        
        return Ok("Password reset email has been sent.");
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(
        [FromQuery] string token,
        [FromBody] ResetPasswordDto dto)
    {
        var command = new ResetPasswordCommand(token, dto.NewPassword);
        var result = await _mediator.Send(command);
        
        if (result.IsFailed)
            return BadRequest(result.Errors);
        
        return Ok("User successfully reset password.");
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