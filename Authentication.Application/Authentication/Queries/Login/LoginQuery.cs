using FluentResults;
using JwtAuthenticationManager.Models;
using MediatR;

namespace Authentication.Application.Authentication.Queries.Login
{
    public record LoginQuery(
        string Login, 
        string Password) : IRequest<Result<JwtTokenResponse>>;
}
