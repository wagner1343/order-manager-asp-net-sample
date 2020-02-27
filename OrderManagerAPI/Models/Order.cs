using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OrderManagerAPI.Models
{
    public class Order : Entity
    {
        [Required]
        public virtual ICollection<Product> Products { get; set; }
        [Required]
        public virtual Client Client { get; set; }
        [Required]
        public double Value { get; set; }
        public double Discount { get; set; }
        [Required]
        public double TotalValue { get; set; }
    }
}