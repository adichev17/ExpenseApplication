using AuthenticationApiService.Errors.ControlError;
using AuthenticationApiService.Models.CommunicationModel;
using AuthenticationApiService.Services;
using JwtAuthenticationManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationApiService.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AccountController : ApiController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IJwtTokenHandler _jwtTokenHandler;
        public AccountController(IAuthenticationService authenticationService, IJwtTokenHandler jwtTokenHandler)
        {
            _authenticationService = authenticationService;
            _jwtTokenHandler = jwtTokenHandler;
        }

        [HttpPost]
        public IActionResult Authenticate([FromBody] AuthenticationRequest authenticationRequest)
        {
            var authenticationResponse = _authenticationService.Authenticate(authenticationRequest.Login, authenticationRequest.Password);
            if (authenticationResponse == null) return Unauthorized();
            return Ok(authenticationResponse);
        }

        [HttpPost]
        public IActionResult Register([FromBody] RegisterRequest registerRequest) 
        {
            var registerResult = _authenticationService.Register(registerRequest.Login, registerRequest.Password);
            if (registerResult.IsSuccess)
            {
                return Ok(registerResult);
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
