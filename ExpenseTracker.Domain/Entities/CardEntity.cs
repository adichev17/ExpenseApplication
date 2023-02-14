using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Domain.Entities
{
    [Table("Card")]
    public class CardEntity : BaseEntity<int>
    {
        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual UserEntity User { get; set; }

        [Required]
        [MaxLength(100)]
        public string CardName { get; set; }

        [Required]
        public int ColorId { get; set; }
        [ForeignKey(nameof(ColorId))]
        public virtual ColorEntity Color { get; set; }

        [Required]
        public decimal Balance { get; set; }
    }
}
