using ExpenseTracker.Application.Common.Interfaces.Repositories;
using ExpenseTracker.Application.Common.Interfaces.Services;
using ExpenseTracker.Application.Common.Mapping;
using Mapster;
using MapsterMapper;
using NSubstitute;
using System.Linq.Expressions;
using ExpenseTracker.Application.Cards.Commands.CreateCard;
using ExpenseTracker.Application.Common.Dtos.Cards;
using ExpenseTracker.Domain.Entities;
using NSubstitute.ReturnsExtensions;
using ExpenseTracker.Application.Common.Errors.Controls;

namespace ExpenseTracker.Tests.Cards.Commands.CreateCard
{
    [TestClass]
    public class CreateCardCommandHandlerTest
    {
        private readonly IUnitOfWork _uowMock;
        private readonly IDateTimeProvider _dateTimeProviderMock;
        private readonly IMapper _mapperMock;

        public CreateCardCommandHandlerTest()
        {
            _uowMock = Substitute.For<IUnitOfWork>();
            _dateTimeProviderMock = Substitute.For<IDateTimeProvider>();

            _mapperMock = Substitute.For<IMapper>();
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(typeof(CardDtoMappingConfig).Assembly);
            _mapperMock = new Mapper(config);
        }

        [TestMethod]
        public async Task WhenUserIsInvalid_ReturnUserNotFoundError()
        {
            //Arrange
            _uowMock.UserRepository.GetByIdAsync(Arg.Any<object>())
                .ReturnsNull();
            var handler = new CreateCardCommandHandler(_uowMock, _dateTimeProviderMock, _mapperMock);
            var createCardCommand = new CreateCardCommand(Guid.NewGuid(), "TestCard", 1);

            //Act
            var result = await handler.Handle(createCardCommand, CancellationToken.None);

            //Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.IsInstanceOfType<UserNotFoundError>(result.Errors.FirstOrDefault());
        }

        [TestMethod]
        public async Task WhenColorNotFound_ReturnColorNotFoundError()
        {
            //Arrange
            _uowMock.UserRepository.GetByIdAsync(Arg.Any<object>())
                .ReturnsForAnyArgs(new UserEntity { });
            _uowMock.ColorRepository.GetByIdAsync(Arg.Any<object>())
                .ReturnsNull();
            var handler = new CreateCardCommandHandler(_uowMock, _dateTimeProviderMock, _mapperMock);
            var createCardCommand = new CreateCardCommand(Guid.NewGuid(), "TestCard", 1);

            //Act
            var result = await handler.Handle(createCardCommand, CancellationToken.None);

            //Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.IsInstanceOfType<ColorNotFoundError>(result.Errors.FirstOrDefault());
        }

        [TestMethod]
        public async Task WhenDuplicateCard_ReturnDuplicateCardError()
        {
            var testUser = new UserEntity
            {
                Id = Guid.Empty,
                CreatedOnUtc = DateTime.Now,
                Login = "Test123",
                Password = "Test123$"
            };
            var colorTest = new ColorEntity
            {
                Id = 1,
                CreatedOnUtc = DateTime.Now,
                ColorName = "Red"
            };

            var userCardTests = new List<CardEntity>
            {
                new()
                {
                    Id = 1,
                    CreatedOnUtc = DateTime.Now,
                    UserId = Guid.Empty,
                    User = testUser,
                    CardName = "TestCard",
                    ColorId = 1,
                    Color = colorTest,
                    Balance = 100
                }
            }.AsQueryable();

            //Arrange
            _uowMock.UserRepository.GetByIdAsync(Arg.Any<object>())
                .ReturnsForAnyArgs(testUser);
            _uowMock.ColorRepository.GetByIdAsync(Arg.Any<object>())
                .Returns(colorTest);
            _uowMock.CardRepository.FindAsync(Arg.Any<Expression<Func<CardEntity, bool>>>())
                .ReturnsForAnyArgs(userCardTests);

            var handler = new CreateCardCommandHandler(_uowMock, _dateTimeProviderMock, _mapperMock);
            var createCardCommand = new CreateCardCommand(Guid.Empty, "TestCard", 1);

            //Act
            var result = await handler.Handle(createCardCommand, CancellationToken.None);

            //Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.IsInstanceOfType<DuplicateCardError>(result.Errors.FirstOrDefault());
        }

        [TestMethod]
        public async Task WhenUserIsValid_ReturnCardDto()
        {
            //Arrange
            var testUser = new UserEntity
            {
                Id = Guid.Empty,
                CreatedOnUtc = DateTime.Now,
                Login = "Test123",
                Password = "Test123$"
            };
            var colorTest = new ColorEntity
            {
                Id = 1,
                CreatedOnUtc = DateTime.Now,
                ColorName = "Red"
            };

            //Arrange
            _uowMock.UserRepository.GetByIdAsync(Arg.Any<object>())
                .ReturnsForAnyArgs(testUser);
            _uowMock.ColorRepository.GetByIdAsync(Arg.Any<object>())
                .Returns(colorTest);
            _uowMock.CardRepository.FindAsync(Arg.Any<Expression<Func<CardEntity, bool>>>())
                .Returns(new List<CardEntity> { }.AsQueryable());

            var handler = new CreateCardCommandHandler(_uowMock, _dateTimeProviderMock, _mapperMock);
            var createCardCommand = new CreateCardCommand(Guid.Empty, "TestCardSecond", 1);

            //Act
            var result = await handler.Handle(createCardCommand, CancellationToken.None);

            //Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsInstanceOfType<CardDto>(result.Value);
            Assert.AreEqual(result.Value.Balance, 0);
            Assert.AreEqual(result.Value.CardName, "TestCardSecond");
            Assert.AreEqual(result.Value.ColorId, 1);
        }
    }
}