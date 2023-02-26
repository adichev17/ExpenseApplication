using ExpenseTracker.API.Models.Communications.Category;
using ExpenseTracker.API.Models.Communications.UserCategory;
using ExpenseTracker.Application.Categories.Commands;
using ExpenseTracker.Application.UserCategories.Commands.CreateUserCategory;
using Mapster;

namespace ExpenseTracker.API.Common.Mapping
{
    public class CreateCategoryMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CreateCategoryRequest, CreateCategoryCommand>();
        }
    }

    public class CreateUserCategoryMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CreateUserCategoryRequest, CreateUserCategoryCommand>();
        }
    }
}
