﻿using System.ComponentModel.DataAnnotations;

namespace Authentication.Domain.Entities
{
    public abstract class BaseEntity<T>
    {
        [Required]
        [Key]
        public T Id { get; set; }

        [Required]
        public DateTime CreatedOnUtc { get; set; }
    }
}
