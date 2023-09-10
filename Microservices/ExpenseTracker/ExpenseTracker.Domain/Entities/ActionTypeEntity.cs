using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Domain.Entities
{
    [Table("ActionType")]
    public class ActionTypeEntity : BaseEntity<int>
    {
        [MaxLength(30)]
        public string ActionTypeName { get; set; }
    }
}
