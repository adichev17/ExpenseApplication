using System.Net;
using System.Runtime.InteropServices.JavaScript;
using Authentication.Application.Authentication.Commands.Register;
using Authentication.Application.Authentication.Queries.Login;
using Authentication.Application.Common.Dtos;
using Authentication.Application.Common.Mapping;
using Authentication.Domain.Common.Errors.ControlError;
using AuthenticationApiService.Controllers;
using AuthenticationApiService.Mapping.Messages;
using AuthenticationApiService.Messages;
using AuthenticationApiService.Models.CommunicationModel;
using FluentResults;
using JwtAuthenticationManager.Models;
using Mapster;
using MapsterMapper;
using MediatR;
using MessageBus.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace Authentication.Tests.Api.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        private readonly IMediator _mediatorMock;
        private readonly IMapper _mapperMock;
        private readonly IMessageProducer _messageProducerMock;

        public AccountControllerTest()
        {
            _messageProducerMock = Substitute.For<IMessageProducer>();
            _mediatorMock = Substitute.For<IMediator>();
            _mapperMock = Substitute.For<IMapper>();
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(typeof(CreateUserDtoMappingProfile).Assembly);
            _mapperMock = new Mapper(config);
        }

        [TestMethod]
        public async Task Authenticate_ReturnOk_WhenCredentialsIsValid()
        {
            //Arrange
            _mediatorMock.Send(Arg.Any<LoginQuery>()).ReturnsForAnyArgs(Result.Ok<JwtTokenResponse>(new JwtTokenResponse
            {
                UserName = "Derri",
                JwtToken = "Token",
                ExpiresIn = 10
            }));
            var controller = new AccountController(_mediatorMock, _mapperMock, _messageProducerMock);
            var authRequest = new AuthenticationRequest
            {
                Login = "Derri",
                Password = "Derri123$"
            };

            //Act
           var result = await controller.Authenticate(authRequest);
           var okResult = result as OkObjectResult;

            //Assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual((int)HttpStatusCode.OK, okResult.StatusCode);
        }


        [TestMethod]
        public async Task Authenticate_ReturnUnauthorized_WhenCredentialsIsInvalid()
        {
            //Arrange
            _mediatorMock.Send(Arg.Any<LoginQuery>())
                .ReturnsForAnyArgs(Result.Fail<JwtTokenResponse>(new InvalidCredentialsError()));
            var controller = new AccountController(_mediatorMock, _mapperMock, _messageProducerMock)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
            var authRequest = new AuthenticationRequest
            {
                Login = "Derri",
                Password = "Derri123$"
            };

            //Act
            var result = await controller.Authenticate(authRequest);
            var objectResult = result as ObjectResult;

            //Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.Unauthorized, objectResult.StatusCode);
        }

        [TestMethod]
        public async Task Register_ReturnConflict_WhenDuplicateLogin()
        {
            //Arrange
            _mediatorMock.Send(Arg.Any<RegisterCommand>())
                .ReturnsForAnyArgs(Result.Fail<RegisterResultDto>(new DuplicateLoginError()));
            var controller = new AccountController(_mediatorMock, _mapperMock, _messageProducerMock)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
            var registerRequest = new RegisterRequest()
            {
                Login = "Derri",
                Password = "Derri123$"
            };

            //Act
            var result = await controller.Register(registerRequest);
            var objectResult = result as ObjectResult;

            //Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.Conflict, objectResult.StatusCode);
        }

        [TestMethod]
        public async Task Register_ReturnOk_WhenRequestIsValid()
        {
            //Arrange
            var registerRequest = new RegisterRequest()
            {
                Login = "Derri",
                Password = "Derri123$"
            };
            _mediatorMock.Send(Arg.Any<RegisterCommand>())
                .ReturnsForAnyArgs(Result.Ok(new RegisterResultDto
                {
                    Id = default,
                    Login = registerRequest.Login,
                    Password = registerRequest.Password,
                    CreatedOnUtc = default
                }));
            var controller = new AccountController(_mediatorMock, _mapperMock, _messageProducerMock)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
            
            //Act
            var result = await controller.Register(registerRequest);
            var objectResult = result as OkResult;

            //Assert
            _messageProducerMock.Received().SendMessage(Arg.Any<CreateUserDto>(), Arg.Any<string>(), Arg.Any<string>());
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.OK, objectResult.StatusCode);
        }
    }
}
