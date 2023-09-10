using ExpenseTracker.Application.Common.Dtos.Categories;
using ExpenseTracker.Application.Common.Errors.Controls;
using ExpenseTracker.Application.Common.Interfaces.Repositories;
using ExpenseTracker.Domain.Entities;
using Mapster;
using MapsterMapper;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System.Linq.Expressions;
using ExpenseTracker.Application.UserCategories.Queries.ListUserCategories;

namespace ExpenseTracker.Tests.UserCategories.Queries.ListUserCategories
{
    [TestClass]
    public class ListUserCategoriesQueryHandlerTest
    {
        private readonly IUnitOfWork _uowMock;
        private readonly IMapper _mapperMock;

        public ListUserCategoriesQueryHandlerTest()
        {
            _uowMock = Substitute.For<IUnitOfWork>();
            _mapperMock = Substitute.For<IMapper>();
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(typeof(CategoryDto).Assembly);
            _mapperMock = new Mapper(config);
        }

        [TestMethod]
        public async Task WhenUserNotExist_ReturnUserNotFoundError()
        {
            //Arrange
            _uowMock.UserRepository.GetByIdAsync(Arg.Any<object>()).ReturnsNull();
            var handler = new ListUserCategoriesQueryHandler(_uowMock, _mapperMock);
            var listUserCategoriesQuery = new ListUserCategoriesQuery(Guid.Empty, 1);

            //Act
            var result = await handler.Handle(listUserCategoriesQuery, CancellationToken.None);

            //Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.IsInstanceOfType<UserNotFoundError>(result.Errors.FirstOrDefault());
        }

        [TestMethod]
        public async Task WhenActionTypeNotExist_ReturnActionTypeNotFoundError()
        {
            //Arrange
            _uowMock.UserRepository.GetByIdAsync(Arg.Any<object>()).ReturnsForAnyArgs(new UserEntity());
            _uowMock.ActionTypeRepository.FindAsync(Arg.Any<Expression<Func<ActionTypeEntity, bool>>>())
                .ReturnsForAnyArgs(Enumerable.Empty<ActionTypeEntity>().AsQueryable());
            var handler = new ListUserCategoriesQueryHandler(_uowMock, _mapperMock);
            var listUserCategoriesQuery = new ListUserCategoriesQuery(Guid.Empty, 1);

            //Act
            var result = await handler.Handle(listUserCategoriesQuery, CancellationToken.None);

            //Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.IsInstanceOfType<ActionTypeNotFoundError>(result.Errors.FirstOrDefault());
        }

        [TestMethod]
        public async Task WhenRequestIsValid_ReturnActionTypeNotFoundError()
        {
            //Arrange
            var user = new UserEntity
            {
                Id = Guid.Empty,
                CreatedOnUtc = default,
                Login = "Test",
                Password = "Test"
            };
            var actionType = new ActionTypeEntity
            {
                Id = 1,
                CreatedOnUtc = default,
                ActionTypeName = "Expense"
            };
            var animalCategory = new CategoryEntity
            {
                Id = 1,
                CreatedOnUtc = default,
                CategoryName = "Animals",
                ImageUri = null,
                ActionTypeId = actionType.Id,
                ActionType = actionType
            };
            var foodCategory = new CategoryEntity
            {
                Id = 2,
                CreatedOnUtc = default,
                CategoryName = "Food",
                ImageUri = null,
                ActionTypeId = actionType.Id,
                ActionType = actionType
            };
            var userCategories = new List<UserCategoryEntity>()
            {
                new()
                {
                    Id = 1,
                    CreatedOnUtc = default,
                    UserId = user.Id,
                    User = user,
                    CategoryId = animalCategory.Id,
                    Category = animalCategory
                },
                new()
                {
                    Id = 2,
                    CreatedOnUtc = default,
                    UserId = user.Id,
                    User = user,
                    CategoryId = foodCategory.Id,
                    Category = foodCategory
                }
            };
            _uowMock.UserRepository.GetByIdAsync(Arg.Any<object>()).ReturnsForAnyArgs(user);
            _uowMock.ActionTypeRepository.GetByIdAsync(Arg.Any<Expression<Func<ActionTypeEntity, bool>>>())
                .ReturnsForAnyArgs(actionType);
            _uowMock.UserCategoryRepository.FindAsync(Arg.Any<Expression<Func<UserCategoryEntity, bool>>>())
                .ReturnsForAnyArgs(userCategories.AsQueryable());
            var handler = new ListUserCategoriesQueryHandler(_uowMock, _mapperMock);
            var listUserCategoriesQuery = new ListUserCategoriesQuery(Guid.Empty, 1);

            //Act
            var result = await handler.Handle(listUserCategoriesQuery, CancellationToken.None);

            //Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(result.Value.Any());
            Assert.AreEqual(result.Value.First().CategoryName, "Animals");
            Assert.AreEqual(result.Value.Skip(1).First().CategoryName, "Food");
        }
    }
}
