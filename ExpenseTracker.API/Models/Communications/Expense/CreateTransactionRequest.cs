namespace ExpenseTracker.API.Models.Communications.Expense
{
    public class CreateTransactionRequest
    {
        public int CardId { get; set; }
        public decimal Amount { get; set; }
        public string Comment { get; set; }
        public int CategoryId { get; set; }
    }
}
