using Authentication.Application.Common.Interfaces.Repositories;
using Authentication.Application.Common.Interfaces.Services;
using Authentication.Domain.Common.Errors.ControlError;
using Authentication.Domain.Entities;
using FluentResults;
using JwtAuthenticationManager;
using JwtAuthenticationManager.Models;
using MediatR;

namespace Authentication.Application.Authentication.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<JwtTokenResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IJwtTokenHandler _jwtTokenHandler;
        public RegisterCommandHandler(
            IUnitOfWork unitOfWork, 
            IDateTimeProvider dateTimeProvider,
            IJwtTokenHandler jwtTokenHandler)
        {
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
            _jwtTokenHandler = jwtTokenHandler;
        }


        public async Task<Result<JwtTokenResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if ((await _unitOfWork.UserRepository.FindAsync(x => x.Login == request.Login)).Any())
            {
                return Result.Fail<JwtTokenResponse>(new DublicateLoginError());
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new UserEntity
            {
                Login = request.Login,
                Password = passwordHash,
                CreatedOnUtc = _dateTimeProvider.UtcNow
            };

            _unitOfWork.UserRepository.Insert(user);
            await _unitOfWork.CommitAsync();

            var generateTokenRequest = new GenerateTokenRequest(user.Id, user.Login, user.Password);
            var token = _jwtTokenHandler.GenerateJwtToken(generateTokenRequest);

            return Result.Ok<JwtTokenResponse>(token);
        }
    }
}
