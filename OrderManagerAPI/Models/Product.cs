using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OrderManagerAPI.Models
{
    public class Product : Entity
    {

        [Required]
        public string Description { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public double price { get; set; }
        public string imageURL { get; set; }

    }
}