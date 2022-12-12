using System;
using System.ComponentModel.DataAnnotations;

namespace TangyRestaurantWebsite.Models.OrderDetailsViewModel
{
	public class OrderExportViewModel
	{
        [Display(Name = "Start Date")]
        public DateTime startDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime endDate { get; set; }
    }
}

