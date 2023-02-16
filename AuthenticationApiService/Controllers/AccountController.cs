using Authentication.Application.Authentication.Commands.Register;
using Authentication.Application.Authentication.Queries.Login;
using Authentication.Domain.Common.Errors.ControlError;
using AuthenticationApiService.Models.CommunicationModel;
using AuthenticationApiService.Services;
using JwtAuthenticationManager;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AuthenticationApiService.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AccountController : ApiController
    {
        private readonly IMediator _mediator;
        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
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
                    break;
            }
            return Problem();
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest) 
        {
            var command = new RegisterCommand(Login: registerRequest.Login, Password: registerRequest.Password);

            var registerResult = await _mediator.Send(command);
            if (registerResult.IsSuccess)
            {
                return Ok(registerResult.Value);
            }

            var error = registerResult.Errors.FirstOrDefault();

            switch (error)
            {
                case DublicateLoginError:
                    return Problem(error, System.Net.HttpStatusCode.Conflict);
                default:
                    break;
            }
            return Problem();
        }


        //[Authorize]
        //[HttpGet]
        //public void Test()
        //{
        //    var temp = int.MaxValue;
        //}
    }
}
