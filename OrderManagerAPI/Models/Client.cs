﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OrderManagerAPI.Models
{
    public class Client : Entity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }

    }
}