using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TangyRestaurantWebsite.Data;
using Microsoft.AspNetCore.Hosting;
using TangyRestaurantWebsite.Models.MenuItemViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TangyRestaurantWebsite.Controllers
{
    public class MenuItemsController : Controller
    {
        // GET: /<controller>/
        private readonly ApplicationDbContext _db;
        private readonly IHostEnvironment _hostingEnvironment;
        [BindProperty]
        public MenuItemViewModel MenuItemVM { get; set; }


        public MenuItemsController(ApplicationDbContext db, IHostEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
            MenuItemVM = new MenuItemViewModel
            {
                Category = _db.Category.ToList(),
                MenuItem = new Models.MenuItem()
            };


        }

        public IActionResult Index()
        {
            return View();
        }
    }
}

