using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Domain.Entities
{
    [Table("Transaction")]
    public class TransactionEntity : BaseEntity<int>
    {
        [Required]
        public int CardId { get; set; }

        [ForeignKey(nameof(CardId))]
        public virtual CardEntity Card { get; set; }

        [MaxLength(20)]
        public string Comment { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public CategoryEntity Category { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
