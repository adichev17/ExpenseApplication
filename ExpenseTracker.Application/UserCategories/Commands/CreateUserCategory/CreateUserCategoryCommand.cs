using ExpenseTracker.Application.Common.Dtos.UserCategories;
using FluentResults;
using MediatR;

namespace ExpenseTracker.Application.UserCategories.Commands.CreateUserCategory
{
    public record CreateUserCategoryCommand(
        int UserId,
        string CategoryName,
        int ActionTypeId) : IRequest<Result<UserCategoryDto>>;
}
