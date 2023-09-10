using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Domain.Entities
{
    [Table("Color")]
    public class ColorEntity : BaseEntity<int>
    {
        [Required]
        [MaxLength(50)]
        public string ColorName { get; set; }
    }
}
