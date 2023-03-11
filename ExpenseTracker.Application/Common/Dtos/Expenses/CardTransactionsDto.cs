using ExpenseTracker.Application.Common.Dtos.Cards;

namespace ExpenseTracker.Application.Common.Dtos.Expenses
{
    public class CardTransactionsDto
    {
        public CardDto Card { get; set; }
        public List<TransactionDto> Transactions { get; set; }
    }
}
