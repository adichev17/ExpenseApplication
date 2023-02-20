namespace ExpenseTracker.Application.Common.Dtos.Cards
{
    public class CreateCardDto
    {
        public int UserId { get; set; }
        public decimal Balance { get; set; }
        public string CardName { get; set; }
        public int ColorId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
