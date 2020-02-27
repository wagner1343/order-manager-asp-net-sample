using System;
using System.ComponentModel.DataAnnotations;

namespace OrderManagerAPI.Models
{
    public abstract class Entity
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}