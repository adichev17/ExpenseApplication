using System.Text.Json.Serialization;

namespace ExpenseTracker.Application.Common.Dtos.Cards
{
    public class CardDto
    {
        public int Id { get; set; } 
        public decimal Balance { get; set; }
        [JsonPropertyName("name")]
        public string CardName { get; set; }
        public int ColorId { get; set; }
        [JsonPropertyName("created")]
        public DateTime CreatedOnUtc { get; set; }
    }
}
