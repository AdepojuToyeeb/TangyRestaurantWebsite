using System;
using Microsoft.AspNetCore.Identity;

namespace TangyRestaurantWebsite.Models
{
	public class ApplicationUser:IdentityUser
	{
        [PersonalData]
        public string? FirstName { get; set; }

        [PersonalData]
        public string LastName { get; set; }
    }
}

