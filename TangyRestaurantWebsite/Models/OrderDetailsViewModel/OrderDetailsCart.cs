using System;
namespace TangyRestaurantWebsite.Models.OrderDetailsViewModel
{
	public class OrderDetailsCart
	{
        public List<ShoppingCart>? listCart { get; set; }
        public OrderHeader? OrderHeader { get; set; }
    }
}

