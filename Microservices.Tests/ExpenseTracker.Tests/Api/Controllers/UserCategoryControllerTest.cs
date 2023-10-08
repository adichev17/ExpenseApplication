using ExpenseTracker.API.Common.Mapping;
using ExpenseTracker.API.Controllers;
using ExpenseTracker.API.Models.Communications.UserCategory;
using ExpenseTracker.Application.Common.Dtos.UserCategories;
using ExpenseTracker.Application.Common.Errors.Controls;
using ExpenseTracker.Application.Common.Interfaces.Services;
using ExpenseTracker.Application.UserCategories.Commands.CreateUserCategory;
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
    public class UserCategoryControllerTest
    {
        private readonly IMediator _mediatorMock;
        private readonly IMapper _mapperMock;
        private readonly IUserProvider _userProvider;

        public UserCategoryControllerTest()
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
        public async Task Create_ReturnOk_WhenRequestIsValid()
        {
            //Arrange
            _mediatorMock.Send(Arg.Any<CreateUserCategoryCommand>()).ReturnsForAnyArgs(Result.Ok<UserCategoryDto>(new UserCategoryDto
            {
                Id = 1,
                CategoryName = string.Empty,
                ActionTypeId = 1
            }));
            var userCategoryController = new UserCategoryController(_mapperMock, _mediatorMock, _userProvider)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
            var createUserCategoryRequest = new CreateUserCategoryRequest
            {
                CategoryName = string.Empty,
                ActionTypeId = 1
            };

            //Act
            var result = await userCategoryController.Create(createUserCategoryRequest);

            //Assert
            var okResult = result as OkResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [TestMethod]
        [DataTestMethod]
        [DynamicData(nameof(GetTestStrings), DynamicDataSourceType.Method)]
        public async Task Create_ReturnHttpStatusCode_WhenError(Error target, HttpStatusCode expectedResult)
        {
            //Arrange
            _mediatorMock.Send(Arg.Any<CreateUserCategoryCommand>()).ReturnsForAnyArgs(Result.Fail(target));
            var userCategoryController = new UserCategoryController(_mapperMock, _mediatorMock, _userProvider)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
            var createUserCategoryRequest = new CreateUserCategoryRequest
            {
                CategoryName = string.Empty,
                ActionTypeId = 1
            };

            //Act
            var result = await userCategoryController.Create(createUserCategoryRequest);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)expectedResult, objectResult.StatusCode);
        }

        private static IEnumerable<object?[]> GetTestStrings()
        {
            yield return new object?[] { new UserNotFoundError(), HttpStatusCode.NotFound };
            yield return new object?[] { new ActionTypeNotFoundError(), HttpStatusCode.NotFound };
            yield return new object?[] { new DuplicateUserCategoryError(), HttpStatusCode.Conflict };;
            yield return new object?[] { new Error("unexpected fail"), HttpStatusCode.InternalServerError };
        }
    }
}
