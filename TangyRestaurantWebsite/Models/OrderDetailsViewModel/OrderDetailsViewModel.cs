﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace TangyRestaurantWebsite.Models.OrderDetailsViewModel
{
	public class OrderDetailsViewModel
	{
        public OrderHeader? OrderHeader { get; set; }
        public List<OrderDetails>? OrderDetail { get; set; }
    }
}

