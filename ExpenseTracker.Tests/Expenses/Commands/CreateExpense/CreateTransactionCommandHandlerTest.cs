using ExpenseTracker.Application.Common.Errors.Controls;
using ExpenseTracker.Application.Common.Interfaces.Repositories;
using ExpenseTracker.Application.Common.Interfaces.Services;
using ExpenseTracker.Domain.Entities;
using NSubstitute;
using System.Linq.Expressions;
using ExpenseTracker.Application.Expenses.Commands.CreateExpense;
using NSubstitute.ReturnsExtensions;

namespace ExpenseTracker.Tests.Expenses.Commands.CreateExpense
{
    [TestClass]
    public class CreateTransactionCommandHandlerTest
    {
        private readonly IUnitOfWork _uowMock;
        private readonly IDateTimeProvider _dateTimeProviderMock;
        private readonly ITransactionRepository _transactionRepositoryMock;

        public CreateTransactionCommandHandlerTest()
        {
            _uowMock = Substitute.For<IUnitOfWork>();
            _dateTimeProviderMock = Substitute.For<IDateTimeProvider>();
            _transactionRepositoryMock = Substitute.For<ITransactionRepository>();
        }

        [TestMethod]
        public async Task WhenCardNotExist_ReturnCardNotFoundError()
        {
            //Arrange
            _uowMock.CardRepository.GetByIdAsync(Arg.Any<Expression<Func<CardEntity, bool>>>()).ReturnsNull();
            var handler = new CreateTransactionCommandHandler(_uowMock, _transactionRepositoryMock, _dateTimeProviderMock);
            var editCardCommand = new CreateTransactionCommand(1, 10, "", 1);

            //Act
            var result = await handler.Handle(editCardCommand, CancellationToken.None);

            //Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.IsInstanceOfType<CardNotFoundError>(result.Errors.FirstOrDefault());
        }

        [TestMethod]
        public async Task WhenUserCategoryNotExist_ReturnUserCategoryNotFoundError()
        {
            //Arrange
            _uowMock.CardRepository.GetByIdAsync(Arg.Any<Expression<Func<CardEntity, bool>>>()).ReturnsForAnyArgs(new CardEntity());
            _uowMock.UserCategoryRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<UserCategoryEntity, bool>>>()).ReturnsNull();
            var handler = new CreateTransactionCommandHandler(_uowMock, _transactionRepositoryMock, _dateTimeProviderMock);
            var editCardCommand = new CreateTransactionCommand(1, 10, "", 1);

            //Act
            var result = await handler.Handle(editCardCommand, CancellationToken.None);

            //Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.IsInstanceOfType<UserCategoryNotFoundError>(result.Errors.FirstOrDefault());
        }


        [TestMethod]
        public async Task WhenRequestIsValid_ReturnOk()
        {
            //Arrange
            _uowMock.CardRepository.GetByIdAsync(Arg.Any<Expression<Func<CardEntity, bool>>>()).ReturnsForAnyArgs(new CardEntity
            {
                Id = 1,
                CreatedOnUtc = default,
                UserId = Guid.Empty,
                CardName = "TestCard",
                ColorId = 1,
                Balance = 100
            });
            _uowMock.UserCategoryRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<UserCategoryEntity, bool>>>())
                .ReturnsForAnyArgs(new UserCategoryEntity
                {
                    Id = 1,
                    CreatedOnUtc = default,
                    UserId = Guid.Empty,
                    CategoryId = 1,
                });
            var handler = new CreateTransactionCommandHandler(_uowMock, _transactionRepositoryMock, _dateTimeProviderMock);
            var editCardCommand = new CreateTransactionCommand(1, 10, "", 1);

            //Act
            var result = await handler.Handle(editCardCommand, CancellationToken.None);

            //Assert
            _transactionRepositoryMock.Received().AddTransaction(Arg.Any<TransactionEntity>());
            Assert.IsTrue(result.IsSuccess);
        }
    }
}
