﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrderManagerAPI.Models
{
    public class Product : Entity
    {

        [Required]
        public string Description { get; set; }
        [Required]
        [Range(double.Epsilon, double.MaxValue)]
        public double Price { get; set; }
        public string ImageURL { get; set; }
        public virtual ICollection<OrderProduct> Orders { get; set; }

    }
}