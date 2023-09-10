using Authentication.Application.Common.Interfaces.Repositories;
using NSubstitute;
using System.Linq.Expressions;
using Authentication.Application.Authentication.Queries.Login;
using Authentication.Domain.Entities;
using JwtAuthenticationManager;
using Authentication.Domain.Common.Errors.ControlError;
using JwtAuthenticationManager.Models;

namespace Authentication.Tests.Application.Queries
{
    [TestClass]
    public class LoginQueryHandlerTest
    {
        private readonly IUnitOfWork _uowMock;
        private readonly IJwtTokenHandler _jwtTokenHandlerMock;

        public LoginQueryHandlerTest()
        {
            _uowMock = Substitute.For<IUnitOfWork>();
            _jwtTokenHandlerMock = Substitute.For<IJwtTokenHandler>();
        }

        [TestMethod]
        public async Task WhenUserLoginNotExist_ReturnInvalidCredentialsError()
        {
            //Arrange
            var testData = new List<UserEntity> { };
            _uowMock.UserRepository.FindAsync(Arg.Any<Expression<Func<UserEntity, bool>>>())
                .Returns(testData.AsQueryable());
            var handler = new LoginQueryHandler(_uowMock, _jwtTokenHandlerMock);
            var loginQuery = new LoginQuery("Test123", "Test123$");

            //Act
            var result = await handler.Handle(loginQuery, CancellationToken.None);

            //Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.IsInstanceOfType<InvalidCredentialsError>(result.Errors.FirstOrDefault());
        }

        [TestMethod]
        public async Task WhenInvalidPassword_ReturnInvalidCredentialsError()
        {
            //Arrange
            var testData = new List<UserEntity>
            {
                new()
                {
                    Id = Guid.NewGuid(), Login = "Test123",
                    Password = "$2a$11$jasQcc8hVWS5baqihMy3BOCRHIjgGicbU09asIm.XU5igvLiXIIZ6"
                }
            };
            _uowMock.UserRepository.FindAsync(Arg.Any<Expression<Func<UserEntity, bool>>>())
                .Returns(testData.AsQueryable());
            var handler = new LoginQueryHandler(_uowMock, _jwtTokenHandlerMock);
            var loginQuery = new LoginQuery("Test123", "Test1234$");

            //Act
            var result = await handler.Handle(loginQuery, CancellationToken.None);

            //Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.IsInstanceOfType<InvalidCredentialsError>(result.Errors.FirstOrDefault());
        }

        [TestMethod]
        public async Task WhenUserIsValid_ReturnJwtTokenResponse()
        {
            //Arrange
            var testData = new List<UserEntity>
            {
                new()
                {
                    Id = Guid.NewGuid(), Login = "Test123",
                    Password = "$2a$11$jasQcc8hVWS5baqihMy3BOCRHIjgGicbU09asIm.XU5igvLiXIIZ6"
                }
            };
            _jwtTokenHandlerMock.GenerateJwtToken(Arg.Any<GenerateTokenRequest>()).Returns(new JwtTokenResponse
            {
                UserName = "Test123",
                ExpiresIn = 10,
                JwtToken = "test-jwt-token"
            });
            _uowMock.UserRepository.FindAsync(Arg.Any<Expression<Func<UserEntity, bool>>>())
                .Returns(testData.AsQueryable());
            var handler = new LoginQueryHandler(_uowMock, _jwtTokenHandlerMock);
            var loginQuery = new LoginQuery("Test123", "Test123$");

            //Act
            var result = await handler.Handle(loginQuery, CancellationToken.None);

            //Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsInstanceOfType<JwtTokenResponse>(result.Value);
            Assert.AreEqual(result.Value.JwtToken, "test-jwt-token");
            Assert.AreEqual(result.Value.UserName, "Test123");
        }
    }
}