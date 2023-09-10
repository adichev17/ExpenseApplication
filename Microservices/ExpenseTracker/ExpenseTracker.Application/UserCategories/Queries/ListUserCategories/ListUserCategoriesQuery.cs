using ExpenseTracker.Application.Common.Dtos.Categories;
using ExpenseTracker.Application.Common.Dtos.UserCategories;
using FluentResults;
using MediatR;

namespace ExpenseTracker.Application.UserCategories.Queries.ListUserCategories
{
    public record ListUserCategoriesQuery(
        Guid UserId, 
        int ActionTypeId) : IRequest<Result<List<CategoryDto>>>;
}
