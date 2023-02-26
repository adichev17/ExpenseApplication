using ExpenseTracker.API.Models.Communications.Expense;
using ExpenseTracker.Application.Expenses.Commands.CreateExpense;
using Mapster;

namespace ExpenseTracker.API.Common.Mapping
{
    public class CreateTransactionMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CreateTransactionRequest, CreateTransactionCommand>();
        }
    }
}
