using ExpenseTracker.Application.Common.Dtos.Cards;
using ExpenseTracker.Application.Common.Dtos.Categories;
using ExpenseTracker.Application.Common.Dtos.Expenses;
using ExpenseTracker.Domain.Entities;
using Mapster;

namespace ExpenseTracker.Application.Common.Mapping
{
    public class TransactionDtoMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<TransactionEntity, TransactionDto>().ConstructUsing(src => new TransactionDto
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
                Card = new CardDto
                {
                    Id = src.Card.Id,
                    Balance = src.Card.Balance,
                    CardName= src.Card.CardName,
                    ColorId = src.Card.ColorId,
                    CreatedOnUtc = src.Card.CreatedOnUtc
                }
            });
        }
    }
}
