using ExpenseTracker.Application.Common.Interfaces.Repositories;
using ExpenseTracker.Application.Common.Mapping;
using ExpenseTracker.Application.Expenses.Queries.ListTransactions;
using ExpenseTracker.Domain.Entities;
using Mapster;
using MapsterMapper;
using NSubstitute;

namespace ExpenseTracker.Tests.Expenses.Queries.ListTransactions
{
    [TestClass]
    public class ListTransactionQueryHandlerTest
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapperMock;
        private readonly TransactionEntity _transactionEntity;

        public ListTransactionQueryHandlerTest()
        {
            _transactionRepository = Substitute.For<ITransactionRepository>();

            _mapperMock = Substitute.For<IMapper>();
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(typeof(TransactionDtoMappingConfig).Assembly);
            _mapperMock = new Mapper(config); 

            _transactionEntity = new TransactionEntity
            { 
                Id = 1,
                CreatedOnUtc = DateTime.Now,
                CardId = 1,
                Card = new CardEntity
                {
                    Id = 1,
                    CreatedOnUtc = DateTime.UtcNow,
                    UserId = Guid.Empty,
                    User = new UserEntity
                    {
                        Id = Guid.Empty,
                        CreatedOnUtc = DateTime.Now,
                        Login = "Test",
                        Password = "Test"
                    },
                    CardName = "test",
                    ColorId = 0,
                    Color = new ColorEntity
                    {
                        Id = 1,
                        CreatedOnUtc = DateTime.Now,
                        ColorName = "Test"
                    },
                    Balance = 0
                },
                Comment = "comment",
                Amount = 0,
                CategoryId = 1,
                Category = new CategoryEntity
                {
                    Id = 1,
                    CreatedOnUtc = DateTime.Now,
                    CategoryName = "test",
                    ImageUri = "test",
                    ActionTypeId = 1,
                    ActionType = new ActionTypeEntity
                    {
                        Id = 1,
                        CreatedOnUtc = DateTime.Now,
                        ActionTypeName = "test"
                    }
                },
                Date = default
            };
        }

        [TestMethod]
        public async Task WhenTransactionsNotFound_ReturnEmptyList()
        {
            //Arrange
            _transactionRepository.GetAll(Arg.Any<Guid>(), Arg.Any<int>(), Arg.Any<int>())
                .ReturnsForAnyArgs(Enumerable.Empty<TransactionEntity>().AsQueryable());
            var handler = new ListTransactionsQueryHandler(_transactionRepository, _mapperMock);
            var query = new ListTransactionsQuery(Guid.Empty, 1, 1);

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(result.Value.Count == 0);
        }

        [TestMethod]
        public async Task WhenTransactionsNotFound_ReturnTransactions()
        {
            //Arrange
            _transactionRepository.GetAll(Arg.Any<Guid>(), Arg.Any<int>(), Arg.Any<int>())
                .ReturnsForAnyArgs(new List<TransactionEntity>()
                {
                    _transactionEntity
                }.AsQueryable());
            var handler = new ListTransactionsQueryHandler(_transactionRepository, _mapperMock);
            var query = new ListTransactionsQuery(Guid.Empty, 1, 1);

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(result.Value.Count, 1);
        }
    }
}