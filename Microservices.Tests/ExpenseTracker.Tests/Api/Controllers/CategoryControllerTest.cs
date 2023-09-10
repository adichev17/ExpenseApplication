using ExpenseTracker.API.Common.Mapping;
using ExpenseTracker.API.Controllers;
using ExpenseTracker.API.Models.Communications.Category;
using ExpenseTracker.Application.Categories.Commands;
using ExpenseTracker.Application.Common.Dtos.Categories;
using ExpenseTracker.Application.Common.Errors.Controls;
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
    public class CategoryControllerTest
    {
        private readonly IMediator _mediatorMock;
        private readonly IMapper _mapperMock;

        public CategoryControllerTest()
        {
            _mediatorMock = Substitute.For<IMediator>();
            _mapperMock = Substitute.For<IMapper>();
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(typeof(CreateCategoryMappingConfig).Assembly);
            _mapperMock = new Mapper(config);
        }

        [TestMethod]
        public async Task Create_ReturnOk_WhenCategoryIsValid()
        {
            //Arrange
            _mediatorMock.Send(Arg.Any<CreateCategoryCommand>()).ReturnsForAnyArgs(Result.Ok(new CategoryDto
            {
                Id = 1,
                CategoryName = "Test",
                ImageUri = "",
                ActionTypeId = 1
            }));
            var categoryController = new CategoryController(_mapperMock, _mediatorMock)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
            var request = new CreateCategoryRequest
            {
                CategoryName = "Category",
                AcctionTypeId = 1
            };

            //Act
            var result = await categoryController.Create(request);

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
            _mediatorMock.Send(Arg.Any<CreateCategoryCommand>()).ReturnsForAnyArgs(Result.Fail(target));
            var categoryController = new CategoryController(_mapperMock, _mediatorMock)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
            var request = new CreateCategoryRequest
            {
                CategoryName = "Category",
                AcctionTypeId = 1
            };

            //Act
            var result = await categoryController.Create(request);

            //Assert
            //Assert
            var objectResult = result as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)expectedResult, (int)objectResult.StatusCode);
        }

        private static IEnumerable<object?[]> GetTestStrings()
        {
            yield return new object?[] { new DuplicateCategoryError(), HttpStatusCode.Conflict };
            yield return new object?[] { new Error("unexpected fail"), HttpStatusCode.InternalServerError };
        }
    }
}
