using System.Linq.Expressions;
using ExpenseTracker.Application.Cards.Commands.DeleteCard;
using ExpenseTracker.Application.Common.Errors.Controls;
using ExpenseTracker.Application.Common.Interfaces.Repositories;
using ExpenseTracker.Application.Common.Interfaces.Services;
using ExpenseTracker.Application.Common.Mapping;
using ExpenseTracker.Domain.Entities;
using Mapster;
using MapsterMapper;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace ExpenseTracker.Tests.Cards.Commands.DeleteCard
{
    [TestClass]
    public class DeleteCardCommandHandlerTest
    {
        private readonly IUnitOfWork _uowMock;
        private readonly IDateTimeProvider _dateTimeProviderMock;
        private readonly IMapper _mapperMock;

        public DeleteCardCommandHandlerTest()
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
            var handler = new DeleteCardCommandHandler(_uowMock, _dateTimeProviderMock, _mapperMock);
            var deleteCardCommand = new DeleteCardCommand(Guid.NewGuid(), 1);

            //Act
            var result = await handler.Handle(deleteCardCommand, CancellationToken.None);

            //Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.IsInstanceOfType<UserNotFoundError>(result.Errors.FirstOrDefault());
        }

        [TestMethod]
        public async Task WhenCardNotExist_ReturnErrorCardNotFound()
        {
            //Arrange
            _uowMock.UserRepository.GetByIdAsync(Arg.Any<object>())
                .Returns(new UserEntity() { });
            _uowMock.CardRepository.GetByIdAsync(Arg.Any<Expression<Func<CardEntity, bool>>>()).ReturnsNull();
            var handler = new DeleteCardCommandHandler(_uowMock, _dateTimeProviderMock, _mapperMock);
            var createCardCommand = new DeleteCardCommand(Guid.NewGuid(), 1);

            //Act
            var result = await handler.Handle(createCardCommand, CancellationToken.None);

            //Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.IsInstanceOfType<CardNotFoundError>(result.Errors.FirstOrDefault());
        }

        [TestMethod]
        public async Task WhenValidCard_ReturnOk()
        {
            //Arrange
            var testCards = new List<CardEntity>()
            {
                new()
                {
                    Id = 1,
                    CreatedOnUtc = DateTime.Now,
                    UserId = Guid.Empty,
                    CardName = "TestCard",
                    ColorId = 1,
                    Balance = 100
                }
            }.AsQueryable();
            _uowMock.UserRepository.GetByIdAsync(Arg.Any<object>())
                .Returns(new UserEntity() { });
            _uowMock.CardRepository.FindAsync(Arg.Any<Expression<Func<CardEntity, bool>>>()).Returns(testCards);
            _uowMock.CardRepository.Delete(Arg.Any<CardEntity>());
            var handler = new DeleteCardCommandHandler(_uowMock, _dateTimeProviderMock, _mapperMock);
            var createCardCommand = new DeleteCardCommand(Guid.NewGuid(), 1);

            //Act
            var result = await handler.Handle(createCardCommand, CancellationToken.None);

            //Assert
            Assert.IsTrue(result.IsSuccess);
        }
    }
}