using System.Text.Json.Serialization;
using FluentResults;
using HotelBookingSystem.Application.Features.Authentication.DTOs;
using MediatR;

namespace HotelBookingSystem.Application.Features.Authentication.Commands.Login;

public record LoginCommand : IRequest<Result<AuthResultDto>>
{
    public string Email { get; init; }
    public string Password { get; init; }

    [JsonConstructor]
    public LoginCommand(string email, string password)
    {
        Email = email;
        Password = password;
    }
}