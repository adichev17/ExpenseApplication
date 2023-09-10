using Authentication.Application.Authentication.Commands.Register;
using Authentication.Application.Common.Dtos;
using Authentication.Application.Common.Interfaces.Repositories;
using Authentication.Application.Common.Interfaces.Services;
using Authentication.Application.Common.Mapping;
using Authentication.Domain.Common.Errors.ControlError;
using Authentication.Domain.Entities;
using Mapster;
using MapsterMapper;
using NSubstitute;
using System.Linq.Expressions;

namespace Authentication.Tests.Application.Commands.Register
{
    [TestClass]
    public class RegisterCommandHandlerTest
    {
        private readonly IUnitOfWork _uowMock;
        private readonly IDateTimeProvider _dateTimeProviderMock;
        private readonly IMapper _mapperMock;

        public RegisterCommandHandlerTest()
        {
            _uowMock = Substitute.For<IUnitOfWork>();
            _dateTimeProviderMock = Substitute.For<IDateTimeProvider>();

            _mapperMock = Substitute.For<IMapper>();
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(typeof(RegisterResultMappingConfig).Assembly);
            _mapperMock = new Mapper(config);
        }

        [TestMethod]
        public async Task WhenDuplicateUserLogin_ReturnDuplicateUserLoginError()
        {
            //Arrange
            var testUsers = new List<UserEntity> { new() { Id = Guid.NewGuid(), Login = "Test123" } };
            _uowMock.UserRepository.FindAsync(Arg.Any<Expression<Func<UserEntity, bool>>>())
                .Returns(testUsers.AsQueryable());
            var handler = new RegisterCommandHandler(_uowMock, _dateTimeProviderMock, _mapperMock);
            var registerCommand = new RegisterCommand("Test123", "Test123$");

            //Act
            var result = await handler.Handle(registerCommand, CancellationToken.None);

            //Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.IsInstanceOfType<DuplicateLoginError>(result.Errors.FirstOrDefault());
        }

        [TestMethod]
        public async Task WhenNewUserIsValid_ReturnUserDto()
        {
            //Arrange
            var testUsers = new List<UserEntity> { };
            _uowMock.UserRepository.FindAsync(Arg.Any<Expression<Func<UserEntity, bool>>>())
                .Returns(testUsers.AsQueryable());
            var handler = new RegisterCommandHandler(_uowMock, _dateTimeProviderMock, _mapperMock);
            var registerCommand = new RegisterCommand("Test123", "Test123$");

            //Act
            var result = await handler.Handle(registerCommand, CancellationToken.None);

            //Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsInstanceOfType<RegisterResultDto>(result.Value);
        }
    }
}