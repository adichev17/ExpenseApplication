using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ExpenseTracker.API.Models.Communications.Card
{
    public class CreateCardRequest
    {
        [Required]
        public string CardName { get; set; }
        [Required]
        public int ColorId { get; set; }
    }
}
