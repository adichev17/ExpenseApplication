using Authentication.Application.Common.Dtos;
using Authentication.Application.Common.Interfaces.Repositories;
using Authentication.Application.Common.Interfaces.Services;
using Authentication.Domain.Common.Errors.ControlError;
using Authentication.Domain.Entities;
using FluentResults;
using JwtAuthenticationManager;
using JwtAuthenticationManager.Models;
using MapsterMapper;
using MediatR;

namespace Authentication.Application.Authentication.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<RegisterResultDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IMapper _mapper;
        public RegisterCommandHandler(
            IUnitOfWork unitOfWork, 
            IDateTimeProvider dateTimeProvider,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
            _mapper = mapper;
        }


        public async Task<Result<RegisterResultDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if ((await _unitOfWork.UserRepository.FindAsync(x => x.Login == request.Login)).Any())
            {
                return Result.Fail<RegisterResultDto>(new DublicateLoginError());
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

            var registerResult = _mapper.Map<RegisterResultDto>(user);

            return Result.Ok(registerResult);
        }
    }
}
