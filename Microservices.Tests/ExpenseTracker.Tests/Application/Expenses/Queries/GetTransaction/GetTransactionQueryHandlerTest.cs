using ExpenseTracker.Application.Common.Dtos.Expenses;
using ExpenseTracker.Application.Common.Errors.Controls;
using ExpenseTracker.Application.Common.Interfaces.Repositories;
using ExpenseTracker.Application.Common.Mapping;
using ExpenseTracker.Application.Expenses.Queries.GetTransaction;
using ExpenseTracker.Domain.Entities;
using Mapster;
using MapsterMapper;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace ExpenseTracker.Tests.Application.Expenses.Queries.GetTransaction
{
    [TestClass]
    public class GetTransactionQueryHandlerTest
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapperMock;
        private readonly TransactionEntity _transactionEntity;

        public GetTransactionQueryHandlerTest()
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
        public async Task WhenTransactionNotFound_ReturnNotFoundTransactionError()
        {
            //Arrange
            _transactionRepository.GetByIdAsync(Arg.Any<int>()).ReturnsNull();
            var handler = new GetTransactionQueryHandler(_transactionRepository, _mapperMock);
            var query = new GetTransactionQuery(1);

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.IsInstanceOfType<NotFoundTransactionError>(result.Errors.FirstOrDefault());
        }

        [TestMethod]
        public async Task WhenTransactionExist_ReturnTransactionDto()
        {
            //Arrange
            _transactionRepository.GetByIdAsync(Arg.Any<int>()).ReturnsForAnyArgs(_transactionEntity);
            var handler = new GetTransactionQueryHandler(_transactionRepository, _mapperMock);
            var query = new GetTransactionQuery(1);

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsInstanceOfType<TransactionDto>(result.Value);
        }
    }
}