using ExpenseTracker.Application.Cards.Commands.EditCard;
using ExpenseTracker.Application.Common.Errors.Controls;
using ExpenseTracker.Application.Common.Interfaces.Repositories;
using ExpenseTracker.Application.Common.Interfaces.Services;
using ExpenseTracker.Application.Common.Mapping;
using ExpenseTracker.Domain.Entities;
using Mapster;
using MapsterMapper;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System.Linq.Expressions;

namespace ExpenseTracker.Tests.Application.Cards.Commands.EditCard
{
    [TestClass]
    public class EditCardCommandHandlerTest
    {
        private readonly IUnitOfWork _uowMock;
        private readonly IDateTimeProvider _dateTimeProviderMock;
        private readonly IMapper _mapperMock;

        public EditCardCommandHandlerTest()
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
            var handler = new EditCardCommandHandler(_uowMock, _dateTimeProviderMock, _mapperMock);
            var editCardCommand = new EditCardCommand(Guid.Empty, 1, 2, "NewTestCardName");

            //Act
            var result = await handler.Handle(editCardCommand, CancellationToken.None);

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
            var handler = new EditCardCommandHandler(_uowMock, _dateTimeProviderMock, _mapperMock);
            var editCardCommand = new EditCardCommand(Guid.Empty, 1, 2, "NewTestCardName");

            //Act
            var result = await handler.Handle(editCardCommand, CancellationToken.None);

            //Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.IsInstanceOfType<ColorNotFoundError>(result.Errors.FirstOrDefault());
        }

        [TestMethod]
        public async Task WhenCardNotExist_ReturnCardNotFoundError()
        {
            //Arrange
            _uowMock.UserRepository.GetByIdAsync(Arg.Any<object>())
                .Returns(new UserEntity() { });
            _uowMock.CardRepository.GetByIdAsync(Arg.Any<Expression<Func<CardEntity, bool>>>()).ReturnsNull();
            _uowMock.ColorRepository.GetByIdAsync(Arg.Any<object>())
                .Returns(new ColorEntity() { });
            var handler = new EditCardCommandHandler(_uowMock, _dateTimeProviderMock, _mapperMock);
            var editCardCommand = new EditCardCommand(Guid.Empty, 1, 2, "NewTestCardName");

            //Act
            var result = await handler.Handle(editCardCommand, CancellationToken.None);

            //Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.IsInstanceOfType<CardNotFoundError>(result.Errors.FirstOrDefault());
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
                    CardName = "TestCardName",
                    ColorId = 1,
                    Color = colorTest,
                    Balance = 100
                },
                new()
                {
                    Id = 2,
                    CreatedOnUtc = DateTime.Now,
                    UserId = Guid.Empty,
                    User = testUser,
                    CardName = "TestCardNameSecond",
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

            var handler = new EditCardCommandHandler(_uowMock, _dateTimeProviderMock, _mapperMock);
            var editCardCommand = new EditCardCommand(Guid.Empty, 2, 2, "TestCardName");

            //Act
            var result = await handler.Handle(editCardCommand, CancellationToken.None);

            //Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.IsInstanceOfType<DuplicateCardError>(result.Errors.FirstOrDefault());
        }


        [TestMethod]
        public async Task WhenValidCard_ReturnCardDto()
        {
            var testUser = new UserEntity
            {
                Id = Guid.Empty,
                CreatedOnUtc = DateTime.Now,
                Login = "Test123",
                Password = "Test123$"
            };
            var colorTestRed = new ColorEntity
            {
                Id = 1,
                CreatedOnUtc = DateTime.Now,
                ColorName = "Blue"
            };
            var colorTestBlue = new ColorEntity
            {
                Id = 2,
                CreatedOnUtc = DateTime.Now,
                ColorName = "Blue"
            };

            var userCardTests = new List<CardEntity>
            {
                new()
                {
                    Id = 1,
                    CreatedOnUtc = DateTime.Now,
                    UserId = Guid.Empty,
                    User = testUser,
                    CardName = "TestCardName",
                    ColorId = 2,
                    Color = colorTestBlue,
                    Balance = 100
                },
                new()
                {
                    Id = 2,
                    CreatedOnUtc = DateTime.Now,
                    UserId = Guid.Empty,
                    User = testUser,
                    CardName = "TestCardNameSecond",
                    ColorId = 1,
                    Color = colorTestRed,
                    Balance = 100
                }
            }.AsQueryable();

            //Arrange
            _uowMock.UserRepository.GetByIdAsync(Arg.Any<object>())
                .ReturnsForAnyArgs(testUser);
            _uowMock.ColorRepository.GetByIdAsync(Arg.Any<object>())
                .Returns(colorTestBlue);
            _uowMock.CardRepository.FindAsync(Arg.Any<Expression<Func<CardEntity, bool>>>())
                .ReturnsForAnyArgs(userCardTests);

            var handler = new EditCardCommandHandler(_uowMock, _dateTimeProviderMock, _mapperMock);
            var editCardCommand = new EditCardCommand(Guid.Empty, 2, 2, "TestCardNameNew");

            //Act
            var result = await handler.Handle(editCardCommand, CancellationToken.None);

            //Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(result.Value.CardName, "TestCardNameNew");
            Assert.AreEqual(result.Value.ColorId, 2);
        }
    }
}