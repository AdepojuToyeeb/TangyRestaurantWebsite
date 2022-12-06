using System;
using System.ComponentModel.DataAnnotations;

namespace TangyRestaurantWebsite.Models.SubCategoryViewModel
{
	public class SubCategoryAndCategoryViewModel
	{
		public SubCategory SubCategory { get; set; }
        public Category Category { get; set; }
        public List<Category> CategoryList { get; set; }
		public List<string> SubCategoryList { get; set; }

		[Display(Name="New Sub Category")]
		public bool isNew { get; set; }

		public string StatusMessage { get; set; }
	}
}

