using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Domain.Entities
{
    [Table("ErrorLog")]
    public class ErrorLogEntity : BaseEntity<int>
    {
        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual UserEntity User { get; set; }

        [MaxLength(300)]
        public string Uri { get; set; }

        [MaxLength(2000)]
        public string Message { get; set; }

        public string CallStack { get; set; }

        public int HttpStatusCode { get; set; }
    }
}
