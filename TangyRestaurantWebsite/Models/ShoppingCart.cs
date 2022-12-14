using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TangyRestaurantWebsite.Models
{
	public class ShoppingCart
	{
		public int Id { get; set; }

		public string? ApplicationUserId { get; set; }

		public int MenuItemId { get; set; }

		[NotMapped]
		[ForeignKey("MenuItemId")]
		public virtual MenuItem? MenuItem { get; set; }

		[NotMapped]
		[ForeignKey("ApplicationUserId")]
		public virtual ApplicationUser? ApplicationUser { get; set; }

		[Range(1,int.MaxValue,ErrorMessage = "Please enter a value greater than {0}")]
		public int Count { get; set; }

		[NotMapped]
		public string? StatusMessage { get; set; }
	}
}

