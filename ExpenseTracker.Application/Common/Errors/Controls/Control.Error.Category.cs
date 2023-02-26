using FluentResults;

namespace ExpenseTracker.Application.Common.Errors.Controls
{
    public sealed class DuplicateCategoryError : Error
    {
        public DuplicateCategoryError(
            string message = "A category with the same name and type already exists in the system") : base(message) { }
    }

    public sealed class DuplicateUserCategoryError : Error
    {
        public DuplicateUserCategoryError(
            string message = "The user already has this category with the specified type") : base(message) { }
    }


    public sealed class ActionTypeNotFoundError : Error
    {
        public ActionTypeNotFoundError(string message="") : base(message)
        {
            WithMetadata("ActionTypeId", "ActionTypeId should be 1 - expense or 2 - income");
        }
    }

    public sealed class CategoryNotFoundError : Error
    {
        public CategoryNotFoundError(string message = "") : base(message)
        {
            WithMetadata("CategoryId", "The specified id was not found in the system.");
        }
    }
}
