using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrderManagerAPI.Models
{
    public class Client : Entity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}