using System;
using System.ComponentModel.DataAnnotations;

namespace TangyRestaurantWebsite.Models
{
	public class Coupons
	{
		[Required]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		public string CouponType { get; set; }
		public enum ECouponType { Percent = 0, Dollar = 1 }

		[Required]
		public double Discount { get; set; }
		[Required]
		public double MinimumAccount { get; set; }
		[Required]
		public byte[] Picture { get; set; }
		public bool isActive { get; set; }
	}
}

