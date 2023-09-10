using AuthenticationApiService.Messages;
using AuthenticationApiService.Models.CommunicationModel;
using MapsterMapper;
using MediatR;
using MessageBus.Common;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Authentication.Application.Authentication.Commands.Register;
using Authentication.Application.Authentication.Queries.Login;
using Authentication.Domain.Common.Errors.ControlError;

namespace AuthenticationApiService.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AccountController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IMessageProducer _messageProducer;
        public AccountController(
            IMediator mediator,
            IMapper mapper,
            IMessageProducer messageProducer)
        {
            _mediator = mediator;
            _mapper = mapper;
            _messageProducer = messageProducer;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticationRequest authenticationRequest)
        {
            var query = new LoginQuery(Login: authenticationRequest.Login, Password: authenticationRequest.Password);
            var queryResult = await _mediator.Send(query);
            if (queryResult.IsSuccess)
            {
                return Ok(queryResult.Value);
            }

            var error = queryResult.Errors.FirstOrDefault();

            switch (error)
            {
                case InvalidCredentialsError:
                    return Problem(error, HttpStatusCode.Unauthorized);
                default:
                    return Problem(); // throw ex
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest) 
        {
            var command = new RegisterCommand(Login: registerRequest.Login, Password: registerRequest.Password);

            var registerResult = await _mediator.Send(command);
            if (registerResult.IsSuccess)
            {
                var createUserDto = _mapper.Map<CreateUserDto>(registerResult.Value);
                _messageProducer.SendMessage(createUserDto, MessageBusConstants.ExchangeUsers, MessageBusConstants.UserRegisterKey);

                return Ok();
            }

            var error = registerResult.Errors.FirstOrDefault();

            switch (error)
            {
                case DuplicateLoginError:
                    return Problem(error, System.Net.HttpStatusCode.Conflict);
                default:
                    return Problem();
            }
        }
    }
}
