using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TangyRestaurantWebsite.Data;
using Microsoft.AspNetCore.Hosting;
using TangyRestaurantWebsite.Models.MenuItemViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using TangyRestaurantWebsite.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TangyRestaurantWebsite.Controllers
{
    public class MenuItemsController : Controller
    {
        
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

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var menuItems = _db.MenuItems.Include(m => m.Category).Include(m => m.SubCategory);
            return View(await menuItems.ToListAsync());
        }

        //GET : MenuItems Create
        public IActionResult Create()
        {
            return View(MenuItemVM);
        }

        //POST: MenuItems Create
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePOST()
        {
            MenuItemVM.MenuItem.SubCategoryId = Convert.ToInt32(Request.Form["SubCategoryId"].ToString());
            if (!ModelState.IsValid)
            {
                return View(MenuItemVM);
            }

            _db.MenuItems.Add(MenuItemVM.MenuItem);
            await _db.SaveChangesAsync();

            

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }


        public JsonResult GetSubCategory(int CategoryId)
        {
            List<SubCategory> subCategoryList = new List<SubCategory>();

            subCategoryList = (from SubCategory in _db.SubCategory
                               where SubCategory.CategoryId == CategoryId
                               select SubCategory).ToList();

            return Json(new SelectList(subCategoryList, "Id", "Name"));
        }

    }
}

