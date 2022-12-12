using System;
namespace TangyRestaurantWebsite.Models.OrderDetailsViewModel
{
	public class OrderListViewModel
	{
        public IList<OrderDetailsViewModel>? Orders { get; set; }
        public PagingInfo? PagingInfo { get; set; }
    }
}

