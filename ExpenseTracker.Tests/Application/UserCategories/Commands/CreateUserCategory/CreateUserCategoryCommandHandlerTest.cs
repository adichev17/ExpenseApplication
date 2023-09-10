using ExpenseTracker.Application.Common.Dtos.UserCategories;
using ExpenseTracker.Application.Common.Errors.Controls;
using ExpenseTracker.Application.Common.Interfaces.Repositories;
using ExpenseTracker.Application.Common.Interfaces.Services;
using ExpenseTracker.Application.UserCategories.Commands.CreateUserCategory;
using ExpenseTracker.Domain.Entities;
using Mapster;
using MapsterMapper;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System.Linq.Expressions;

namespace ExpenseTracker.Tests.Application.UserCategories.Commands.CreateUserCategory
{
    [TestClass]
    public class CreateUserCategoryCommandHandlerTest
    {
        private readonly IUnitOfWork _uowMock;
        private readonly IDateTimeProvider _dateTimeProviderMock;
        private readonly IMapper _mapperMock;

        public CreateUserCategoryCommandHandlerTest()
        {
            _uowMock = Substitute.For<IUnitOfWork>();
            _dateTimeProviderMock = Substitute.For<IDateTimeProvider>();

            _mapperMock = Substitute.For<IMapper>();
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(typeof(UserCategoryDto).Assembly);
            _mapperMock = new Mapper(config);
        }

        [TestMethod]
        public async Task WhenUserNotExist_ReturnUserNotFoundError()
        {
            //Arrange
            _uowMock.UserRepository.GetByIdAsync(Arg.Any<object>()).ReturnsNull();
            var handler = new CreateUserCategoryCommandHandler(_uowMock, _dateTimeProviderMock, _mapperMock);
            var createUserCategoryCommand = new CreateUserCategoryCommand(Guid.Empty, "TestCategory", 1);

            //Act
            var result = await handler.Handle(createUserCategoryCommand, CancellationToken.None);

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
            var handler = new CreateUserCategoryCommandHandler(_uowMock, _dateTimeProviderMock, _mapperMock);
            var createUserCategoryCommand = new CreateUserCategoryCommand(Guid.Empty, "TestCategory", 1);

            //Act
            var result = await handler.Handle(createUserCategoryCommand, CancellationToken.None);

            //Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.IsInstanceOfType<ActionTypeNotFoundError>(result.Errors.FirstOrDefault());
        }

        [TestMethod]
        public async Task WhenDuplicateUserCategory_ReturnActionTypeNotFoundError()
        {
            //Arrange
            var user = new UserEntity
            {
                Id = Guid.Empty,
                CreatedOnUtc = default,
                Login = "UserTest",
                Password = "UserPassword"
            };
            var category = new CategoryEntity
            {
                Id = 1,
                CreatedOnUtc = default,
                CategoryName = "TestCategory",
                ActionTypeId = 1
            };
            var userCategories = new List<UserCategoryEntity>()
            {
                new()
                {
                    Id = 1,
                    CreatedOnUtc = default,
                    UserId = user.Id,
                    User = user,
                    CategoryId = category.Id,
                    Category = category
                }
            };
            _uowMock.UserRepository.GetByIdAsync(Arg.Any<object>()).ReturnsForAnyArgs(new UserEntity());
            _uowMock.ActionTypeRepository.GetByIdAsync(Arg.Any<Expression<Func<ActionTypeEntity, bool>>>())
                .ReturnsForAnyArgs(new ActionTypeEntity
                {
                    Id = 1,
                    CreatedOnUtc = default,
                    ActionTypeName = "Expense"
                });
            _uowMock.CategoryRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<CategoryEntity, bool>>>())
                .ReturnsForAnyArgs(category);
            _uowMock.UserCategoryRepository.FindAsync(Arg.Any<Expression<Func<UserCategoryEntity, bool>>>())
                .Returns(userCategories.AsQueryable());
            var handler = new CreateUserCategoryCommandHandler(_uowMock, _dateTimeProviderMock, _mapperMock);
            var createUserCategoryCommand = new CreateUserCategoryCommand(Guid.Empty, "TestCategory", 1);

            //Act
            var result = await handler.Handle(createUserCategoryCommand, CancellationToken.None);

            //Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.IsInstanceOfType<DuplicateUserCategoryError>(result.Errors.FirstOrDefault());
        }

        [TestMethod]
        public async Task WhenUserCategoryIsValid_ReturnUserCategoryDto()
        {
            //Arrange
            _uowMock.UserRepository.GetByIdAsync(Arg.Any<object>()).ReturnsForAnyArgs(new UserEntity());
            _uowMock.ActionTypeRepository.GetByIdAsync(Arg.Any<Expression<Func<ActionTypeEntity, bool>>>())
                .ReturnsForAnyArgs(new ActionTypeEntity
                {
                    Id = 1,
                    CreatedOnUtc = default,
                    ActionTypeName = "Expense"
                });
            _uowMock.CategoryRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<CategoryEntity, bool>>>())
                .ReturnsNull();
            var handler = new CreateUserCategoryCommandHandler(_uowMock, _dateTimeProviderMock, _mapperMock);
            var createUserCategoryCommand = new CreateUserCategoryCommand(Guid.Empty, "TestCategory", 1);

            //Act
            var result = await handler.Handle(createUserCategoryCommand, CancellationToken.None);

            //Assert
            _uowMock.CategoryRepository.Received().Insert(Arg.Any<CategoryEntity>());
            _uowMock.UserCategoryRepository.Received().Insert(Arg.Any<UserCategoryEntity>());
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(result.Value.CategoryName, "TestCategory");
            Assert.AreEqual(result.Value.ActionTypeId, 1);
            Assert.IsInstanceOfType<UserCategoryDto>(result.Value);
        }
    }
}