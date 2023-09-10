using ExpenseTracker.API.Models.Communications.Card;
using ExpenseTracker.Application.Cards.Commands.CreateCard;
using Mapster;

namespace ExpenseTracker.API.Common.Mapping
{
    public class CreateCardMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CreateCardRequest, CreateCardCommand>();
        }
    }
}
