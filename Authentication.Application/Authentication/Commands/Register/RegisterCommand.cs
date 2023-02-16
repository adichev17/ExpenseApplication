using FluentResults;
using JwtAuthenticationManager.Models;
using MediatR;

namespace Authentication.Application.Authentication.Commands.Register
{
    public record RegisterCommand(
        string Login,
        string Password) : IRequest<Result<JwtTokenResponse>>;
}
