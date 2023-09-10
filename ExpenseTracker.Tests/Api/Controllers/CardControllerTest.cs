using ExpenseTracker.API.Common.Mapping;
using ExpenseTracker.API.Controllers;
using ExpenseTracker.API.Models.Communications.Card;
using ExpenseTracker.Application.Cards.Commands.CreateCard;
using ExpenseTracker.Application.Common.Dtos.Cards;
using ExpenseTracker.Application.Common.Errors.Controls;
using ExpenseTracker.Application.Common.Interfaces.Services;
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
    public class CardControllerTest
    {
        private readonly IMediator _mediatorMock;
        private readonly IMapper _mapperMock;
        private readonly IUserProvider _userProvider;

        public CardControllerTest()
        {
            _userProvider = Substitute.For<IUserProvider>();
            _userProvider.GetUserId().ReturnsForAnyArgs(Guid.Empty);
            _mediatorMock = Substitute.For<IMediator>();
            _mapperMock = Substitute.For<IMapper>();
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(typeof(CreateCardMappingConfig).Assembly);
            _mapperMock = new Mapper(config);
        }

        [TestMethod]
        public async Task Create_ReturnOk_WhenCardIsValid()
        {
            //Arrange
            _mediatorMock.Send(Arg.Any<CreateCardCommand>()).ReturnsForAnyArgs(Result.Ok<CardDto>(new CardDto
            {
                Id = 1,
                Balance = 100,
                CardName = "Card",
                ColorId = 1,
                CreatedOnUtc = DateTime.Now
            }));
            var cardController = new CardController(_mapperMock, _mediatorMock, _userProvider)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
            var createCardRequest = new CreateCardRequest
            {
                UserId = default,
                CardName = "CardTest",
                ColorId = 1
            };

            //Act
            var result = await cardController.Create(createCardRequest);

            //Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [TestMethod]
        [DataTestMethod]
        [DynamicData(nameof(GetTestStrings), DynamicDataSourceType.Method)]
        public async Task Create_ReturnHttpStatusCode_WhenError(Error target, HttpStatusCode expectedResult)
        {
            //Arrange
            _mediatorMock.Send(Arg.Any<CreateCardCommand>()).ReturnsForAnyArgs(Result.Fail(target));
            var cardController = new CardController(_mapperMock, _mediatorMock, _userProvider)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
            var createCardRequest = new CreateCardRequest
            {
                UserId = default,
                CardName = "CardTest",
                ColorId = 1
            };

            //Act
            var result = await cardController.Create(createCardRequest);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)expectedResult, objectResult.StatusCode);
        }

        private static IEnumerable<object?[]> GetTestStrings()
        {
            yield return new object?[] { new UserNotFoundError(), HttpStatusCode.NotFound };
            yield return new object?[] { new ColorNotFoundError(), HttpStatusCode.NotFound };
            yield return new object?[] { new CardNotFoundError(), HttpStatusCode.NotFound };
            yield return new object?[] { new DuplicateCardError(), HttpStatusCode.Conflict };
            yield return new object?[] { new Error("unexpected fail"), HttpStatusCode.InternalServerError };
        }
    }
}
