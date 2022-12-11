using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TangyRestaurantWebsite.Models
{
	public class SubCategory
	{
		[Key]
		[Required]
		public int Id { get; set; }

		[Required]
		[Display(Name ="Sub Category")]
		public string Name { get; set; }

		[Required]
		[ForeignKey("CategoryId")]
		[Display(Name ="Category")]
		public int CategoryId { get; set; }

		
		public virtual Category? MyCategory { get; set; }
	}
}

