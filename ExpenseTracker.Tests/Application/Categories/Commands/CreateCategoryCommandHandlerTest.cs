using System.Linq.Expressions;
using ExpenseTracker.Application.Categories.Commands;
using ExpenseTracker.Application.Common.Dtos.Categories;
using ExpenseTracker.Application.Common.Errors.Controls;
using ExpenseTracker.Application.Common.Interfaces.Repositories;
using ExpenseTracker.Application.Common.Interfaces.Services;
using ExpenseTracker.Domain.Entities;
using Mapster;
using MapsterMapper;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace ExpenseTracker.Tests.Application.Categories.Commands
{
    [TestClass]
    public class CreateCategoryCommandHandlerTest
    {
        private readonly IUnitOfWork _uowMock;
        private readonly IDateTimeProvider _dateTimeProviderMock;
        private readonly IMapper _mapperMock;

        public CreateCategoryCommandHandlerTest()
        {
            _uowMock = Substitute.For<IUnitOfWork>();
            _dateTimeProviderMock = Substitute.For<IDateTimeProvider>();

            _mapperMock = Substitute.For<IMapper>();
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(typeof(CategoryDto).Assembly);
            _mapperMock = new Mapper(config);
        }

        [TestMethod]
        public async Task WhenActionTypeNotExist_ReturnActionTypeNotFoundError()
        {
            //Arrange
            _uowMock.ActionTypeRepository.GetByIdAsync(Arg.Any<object>()).ReturnsNull();
            var handler = new CreateCategoryCommandHandler(_uowMock, _dateTimeProviderMock, _mapperMock);
            var createCategoryCommand = new CreateCategoryCommand("TestCategory", 1);

            //Act
            var result = await handler.Handle(createCategoryCommand, CancellationToken.None);

            //Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.IsInstanceOfType<ActionTypeNotFoundError>(result.Errors.FirstOrDefault());
        }

        [TestMethod]
        public async Task WhenDuplicateCategory_ReturnDuplicateCategoryError()
        {
            //Arrange
            _uowMock.ActionTypeRepository.GetByIdAsync(Arg.Any<object>()).Returns(new ActionTypeEntity());
            var categories = new List<CategoryEntity>() {new ()
                {
                    Id = 1,
                    CreatedOnUtc = default,
                    CategoryName = "TestCategory",
                    ImageUri = "",
                    ActionTypeId = 1
                }
            };
            _uowMock.CategoryRepository.FindAsync(Arg.Any<Expression<Func<CategoryEntity, bool>>>())
                .ReturnsForAnyArgs(categories.AsQueryable());
            var handler = new CreateCategoryCommandHandler(_uowMock, _dateTimeProviderMock, _mapperMock);
            var createCategoryCommand = new CreateCategoryCommand("TestCategory", 1);

            //Act
            var result = await handler.Handle(createCategoryCommand, CancellationToken.None);

            //Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.IsInstanceOfType<DuplicateCategoryError>(result.Errors.FirstOrDefault());
        }

        [TestMethod]
        public async Task WhenNewCategory_ReturnCategoryDto()
        {
            //Arrange
            _uowMock.ActionTypeRepository.GetByIdAsync(Arg.Any<object>()).Returns(new ActionTypeEntity());
            _uowMock.CategoryRepository.FindAsync(Arg.Any<Expression<Func<CategoryEntity, bool>>>())
                .ReturnsForAnyArgs(Enumerable.Empty<CategoryEntity>().AsQueryable());
            var handler = new CreateCategoryCommandHandler(_uowMock, _dateTimeProviderMock, _mapperMock);
            var createCategoryCommand = new CreateCategoryCommand("TestCategory", 1);

            //Act
            var result = await handler.Handle(createCategoryCommand, CancellationToken.None);

            //Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(result.Value.CategoryName, "TestCategory");
            Assert.AreEqual(result.Value.ActionTypeId, 1);
        }
    }
}
