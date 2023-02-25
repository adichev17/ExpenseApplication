using ExpenseTracker.Application.Cards.Commands.CreateCard;
using ExpenseTracker.Application.Common.Dtos.Cards;
using ExpenseTracker.Domain.Entities;
using Mapster;

namespace ExpenseTracker.Application.Common.Mapping
{
    public class CardDtoMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CardEntity, CardDto>();
        }
    }
}
