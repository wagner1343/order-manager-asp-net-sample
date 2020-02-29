using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrderManagerAPI.Models
{
    public class Order : Entity
    {
        [Required]
        public virtual ICollection<OrderProduct> Products { get; set; }
        [Required]
        public virtual Client Client { get; set; }
        [Required]
        public double Value { get; set; }
        public double Discount { get; set; }
        [Required]
        public double TotalValue { get; set; }
    }
}