using ExpenseTracker.Application.Common.Interfaces.Repositories;
using ExpenseTracker.Application.Common.Interfaces.Services;
using ExpenseTracker.Application.Common.Mapping;
using ExpenseTracker.Domain.Entities;
using Mapster;
using MapsterMapper;
using NSubstitute;
using System.Linq.Expressions;
using ExpenseTracker.Application.Cards.Queries.ListCards;
using ExpenseTracker.Application.Common.Dtos.Cards;

namespace ExpenseTracker.Tests.Cards.Queries.ListCards
{
    [TestClass]
    public class ListCardsQueryHandlerTest
    {
        private readonly IUnitOfWork _uowMock;
        private readonly IDateTimeProvider _dateTimeProviderMock;
        private readonly IMapper _mapperMock;

        public ListCardsQueryHandlerTest()
        {
            _uowMock = Substitute.For<IUnitOfWork>();
            _dateTimeProviderMock = Substitute.For<IDateTimeProvider>();

            _mapperMock = Substitute.For<IMapper>();
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(typeof(CardDtoMappingConfig).Assembly);
            _mapperMock = new Mapper(config);
        }

        [TestMethod]
        public async Task WhenCardsNotExist_ReturnEmptyList()
        {
            //Arrange
            _uowMock.CardRepository.FindAsync(Arg.Any<Expression<Func<CardEntity, bool>>>()).ReturnsForAnyArgs(Enumerable.Empty<CardEntity>().AsQueryable());
            var handler = new ListCardsQueryHandler(_uowMock, _mapperMock);
            var listCardsQuery = new ListCardsQuery(Guid.Empty);

            //Act
            var result = await handler.Handle(listCardsQuery, CancellationToken.None);

            //Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.Value.Any());
        }

        [TestMethod]
        public async Task WhenCardsExist_ReturnCardDtos()
        {
            //Arrange
            var cards = new List<CardEntity>()
            {
                new()
                {
                    Id = 1,
                    Balance = 100,
                    CardName = "TestCardFirst",
                    ColorId = 1,
                    CreatedOnUtc = DateTime.UtcNow
                },
                new()
                {
                    Id = 2,
                    Balance = 100,
                    CardName = "TestCardSecond",
                    ColorId = 1,
                    CreatedOnUtc = DateTime.UtcNow
                }
            }.AsQueryable();
            _uowMock.CardRepository.FindAsync(Arg.Any<Expression<Func<CardEntity, bool>>>()).ReturnsForAnyArgs(cards);
            var handler = new ListCardsQueryHandler(_uowMock, _mapperMock);
            var listCardsQuery = new ListCardsQuery(Guid.Empty);

            //Act
            var result = await handler.Handle(listCardsQuery, CancellationToken.None);

            //Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(result.Value.Any());
            Assert.IsInstanceOfType<List<CardDto>>(result.Value);
        }
    }
}
