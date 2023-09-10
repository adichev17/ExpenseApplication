using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Domain.Entities
{
    [Table("Category")]
    public class CategoryEntity : BaseEntity<int>
    {
        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; }

        [MaxLength(300)]
        public string ImageUri { get; set; }

        [Required]
        public int ActionTypeId { get; set; }

        [ForeignKey(nameof(ActionTypeId))]
        public ActionTypeEntity ActionType { get; set; }
    }
}
