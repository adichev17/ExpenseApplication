namespace ExpenseTracker.API.Models.Communications.Card
{
    public class EditCardRequest
    {
        public int UserId { get; set; }
        public int CardId { get; set; }
        public int ColorId { get; set; }
        public string CardName { get; set; }
    }
}
