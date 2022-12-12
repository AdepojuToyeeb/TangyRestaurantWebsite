using System;
namespace TangyRestaurantWebsite.Models.OrderDetailsViewModel
{
	public class OrderDetailsViewModel
	{
        public OrderHeader? OrderHeader { get; set; }
        public List<OrderDetails>? OrderDetail { get; set; }
    }
}

