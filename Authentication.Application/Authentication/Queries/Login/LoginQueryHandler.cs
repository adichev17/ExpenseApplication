using Authentication.Application.Common.Interfaces.Repositories;
using Authentication.Domain.Common.Errors.ControlError;
using Authentication.Domain.Entities;
using FluentResults;
using JwtAuthenticationManager;
using JwtAuthenticationManager.Models;
using MediatR;

namespace Authentication.Application.Authentication.Queries.Login
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, Result<JwtTokenResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtTokenHandler _jwtTokenHandler;

        public LoginQueryHandler(IUnitOfWork unitOfWork, IJwtTokenHandler jwtTokenHandler)
        {
            _unitOfWork= unitOfWork;
            _jwtTokenHandler= jwtTokenHandler;
        }

        public async Task<Result<JwtTokenResponse>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var tdt = _unitOfWork.UserRepository.GetAll().ToList();
            if ((await _unitOfWork.UserRepository.FindAsync(x => x.Login == request.Login))?.FirstOrDefault() is not UserEntity user) {
                return Result.Fail<JwtTokenResponse>(new InvalidCredentialsError());
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return Result.Fail<JwtTokenResponse>(new InvalidCredentialsError());
            }

            var tokenRequest = new GenerateTokenRequest(user.Id, user.Login, user.Password);
            var token = _jwtTokenHandler.GenerateJwtToken(tokenRequest);

            return Result.Ok<JwtTokenResponse>(token);
        }
    }
}
