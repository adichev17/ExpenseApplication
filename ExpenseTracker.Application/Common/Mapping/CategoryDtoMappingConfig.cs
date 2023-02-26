using ExpenseTracker.Application.Common.Dtos.Cards;
using ExpenseTracker.Application.Common.Dtos.Categories;
using ExpenseTracker.Domain.Entities;
using Mapster;

namespace ExpenseTracker.Application.Common.Mapping
{
    public class CategoryDtoMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CategoryEntity, CategoryDto>();
        }
    }
}