using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.API.Models.Communications.Card
{
    public class CreateCardRequest
    {
        public int UserId { get; set; }
        public string CardName { get; set; }
        public int ColorId { get; set; }
    }
}
