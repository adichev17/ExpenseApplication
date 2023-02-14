using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Domain.Entities
{
    public abstract class BaseEntity<T>
    {
        [Required]
        public T Id { get; set; }

        [Required]
        public DateTime CreatedOnUtc { get; set; }
    }
}
