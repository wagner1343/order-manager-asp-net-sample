using System.ComponentModel.DataAnnotations;

namespace OrderManagerAPI.Models
{
    public class OrderProduct : Entity
    {
        [Required]
        public virtual Order Order { get; set; }
        [Required]
        public virtual Product Product { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int Amount { get; set; }
    }
}