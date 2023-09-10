using ExpenseTracker.API.Common.Mapping;
using ExpenseTracker.API.Controllers;
using ExpenseTracker.API.Models.Communications.Expense;
using ExpenseTracker.Application.Common.Errors.Controls;
using ExpenseTracker.Application.Common.Interfaces.Services;
using ExpenseTracker.Application.Expenses.Commands.CreateExpense;
using FluentResults;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System.Net;

namespace ExpenseTracker.Tests.Api.Controllers
{
    [TestClass]
    public class ExpenseControllerTest
    {
        private readonly IMediator _mediatorMock;
        private readonly IMapper _mapperMock;
        private readonly IUserProvider _userProvider;

        public ExpenseControllerTest()
        {
            _userProvider = Substitute.For<IUserProvider>();
            _userProvider.GetUserId().ReturnsForAnyArgs(Guid.Empty);
            _mediatorMock = Substitute.For<IMediator>();
            _mapperMock = Substitute.For<IMapper>();
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(typeof(CreateTransactionMappingConfig).Assembly);
            _mapperMock = new Mapper(config);
        }

        [TestMethod]
        public async Task CreateTransaction_ReturnOk_WhenRequestIsValid()
        {
            //Arrange
            _mediatorMock.Send(Arg.Any<CreateTransactionCommand>()).ReturnsForAnyArgs(Result.Ok<bool>(true));
            var expenseController = new ExpenseController(_mapperMock, _mediatorMock, _userProvider)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
            var createTransactionRequest = new CreateTransactionRequest
            {
                CardId = 1,
                Amount = 100,
                Comment = string.Empty,
                CategoryId = 1
            };

            //Act
            var result = await expenseController.CreateTransaction(createTransactionRequest);

            //Assert
            var okResult = result as OkResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [TestMethod]
        [DataTestMethod]
        [DynamicData(nameof(GetTestStrings), DynamicDataSourceType.Method)]
        public async Task CreateTransaction_ReturnHttpStatusCode_WhenError(Error target, HttpStatusCode expectedResult)
        {
            //Arrange
            _mediatorMock.Send(Arg.Any<CreateTransactionCommand>()).ReturnsForAnyArgs(Result.Fail(target));
            var expenseController = new ExpenseController(_mapperMock, _mediatorMock, _userProvider)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
            var createTransactionRequest = new CreateTransactionRequest
            {
                CardId = 1,
                Amount = 100,
                Comment = string.Empty,
                CategoryId = 1
            };

            //Act
            var result = await expenseController.CreateTransaction(createTransactionRequest);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)expectedResult, objectResult.StatusCode);
        }

        private static IEnumerable<object?[]> GetTestStrings()
        {
            yield return new object?[] { new CardNotFoundError(), HttpStatusCode.NotFound };
            yield return new object?[] { new CategoryNotFoundError(), HttpStatusCode.NotFound };
            yield return new object?[] { new UserCategoryNotFoundError(), HttpStatusCode.NotFound };
            yield return new object?[] { new NotFoundTransactionError(), HttpStatusCode.NotFound };
            yield return new object?[] { new Error("unexpected fail"), HttpStatusCode.InternalServerError };
        }
    }
}
