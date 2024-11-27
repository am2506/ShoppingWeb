﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Core.Models.OrderComponents
{
    public class DeliveryMethod:BaseEntity
    {
        public string ShortName { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Cost { get; set; } 
        public string? DeliveryTime { get; set; } 
        public OrderStatus OrderStatus { get; set; }

    }
}
