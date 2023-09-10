using ExpenseTracker.Application.Common.Errors.Controls;
using ExpenseTracker.Application.Common.Interfaces.Repositories;
using ExpenseTracker.Application.Common.Interfaces.Services;
using ExpenseTracker.Application.Common.Mapping;
using ExpenseTracker.Domain.Entities;
using Mapster;
using MapsterMapper;
using NSubstitute;
using System.Linq.Expressions;
using ExpenseTracker.Application.Cards.Queries.GetCard;
using ExpenseTracker.Application.Common.Dtos.Cards;

namespace ExpenseTracker.Tests.Cards.Queries.GetCard
{
    [TestClass]
    public class GetCardQueryHandlerTest
    {
        private readonly IUnitOfWork _uowMock;
        private readonly IDateTimeProvider _dateTimeProviderMock;
        private readonly IMapper _mapperMock;

        public GetCardQueryHandlerTest()
        {
            _uowMock = Substitute.For<IUnitOfWork>();
            _dateTimeProviderMock = Substitute.For<IDateTimeProvider>();

            _mapperMock = Substitute.For<IMapper>();
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(typeof(CardDtoMappingConfig).Assembly);
            _mapperMock = new Mapper(config);
        }

        [TestMethod]
        public async Task WhenCardNotExist_ReturnCardNotFoundError()
        {
            //Arrange
            _uowMock.CardRepository.FindAsync(Arg.Any<Expression<Func<CardEntity, bool>>>()).ReturnsForAnyArgs(Enumerable.Empty<CardEntity>().AsQueryable());
            var handler = new GetCardQueryHandler(_uowMock, _mapperMock);
            var getCardQuery = new GetCardQuery(Guid.Empty, 1);

            //Act
            var result = await handler.Handle(getCardQuery, CancellationToken.None);

            //Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.IsInstanceOfType<CardNotFoundError>(result.Errors.FirstOrDefault());
        }

        [TestMethod]
        public async Task WhenCardIsValid_ReturnCardDto()
        {
            //Arrange
            _uowMock.CardRepository.FindAsync(Arg.Any<Expression<Func<CardEntity, bool>>>()).ReturnsForAnyArgs(
                new List<CardEntity>
                {
                    new()
                    {
                        Id = 1,
                        CreatedOnUtc = DateTime.UtcNow,
                        UserId = Guid.Empty,
                        CardName = "TestCard",
                        ColorId = 1,
                        Balance = 100
                    }
                }.AsQueryable());
            var handler = new GetCardQueryHandler(_uowMock, _mapperMock);
            var getCardQuery = new GetCardQuery(Guid.Empty, 1);

            //Act
            var result = await handler.Handle(getCardQuery, CancellationToken.None);

            //Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsInstanceOfType<CardDto>(result.Value);
            Assert.AreEqual(result.Value.CardName, "TestCard");
            Assert.AreEqual(result.Value.Balance, 100);
        }
    }
}
