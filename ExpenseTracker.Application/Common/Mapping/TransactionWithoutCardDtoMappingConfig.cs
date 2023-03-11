using ExpenseTracker.Application.Common.Dtos.Categories;
using ExpenseTracker.Application.Common.Dtos.Expenses;
using ExpenseTracker.Domain.Entities;
using Mapster;

namespace ExpenseTracker.Application.Common.Mapping
{
    public class TransactionWithoutCardDtoMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<TransactionEntity, TransactionWithoutCardDto>().ConstructUsing(src => new TransactionWithoutCardDto
            {
                Id = src.Id,
                Date = src.Date,
                Amount = src.Amount,
                Category = new CategoryDto
                {
                    Id = src.Category.Id,
                    ActionTypeId = src.Category.ActionTypeId,
                    CategoryName = src.Category.CategoryName,
                    ImageUri = src.Category.ImageUri
                },
                Comment = src.Comment,
            });
        }
    }
}
