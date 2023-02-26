using ExpenseTracker.Application.Common.Dtos.Categories;
using FluentResults;
using MediatR;

namespace ExpenseTracker.Application.Categories.Commands
{
    public record CreateCategoryCommand (
        string CategoryName,
        int ActionTypeId) : IRequest<Result<CategoryDto>>;
}
