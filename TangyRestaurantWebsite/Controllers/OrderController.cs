using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TangyRestaurantWebsite.Data;
using TangyRestaurantWebsite.Models.OrderDetailsViewModel;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TangyRestaurantWebsite.Controllers
{
    public class OrderController : Controller
    {
        private ApplicationDbContext _db;

        public OrderController(ApplicationDbContext db)
        {
            _db = db;
        }


        //CONFIRM GET
        [Authorize]
        public async Task<IActionResult> Confirm(int id)
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);


            OrderDetailsViewModel OrderDetailsViewModel = new OrderDetailsViewModel()
            {
                OrderHeader = _db.OrderHeader.Where(o => o.Id == id && o.UserId == claim.Value).FirstOrDefault(),
                OrderDetail = _db.OrderDetail.Where(o => o.OrderId == id).ToList()
            };

            return View(OrderDetailsViewModel);
        }
    }
}

