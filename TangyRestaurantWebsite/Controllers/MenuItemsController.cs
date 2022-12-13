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
using TangyRestaurantWebsite.Utility;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.Extensions.Hosting.Internal;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TangyRestaurantWebsite.Controllers
{
    [Authorize(Roles = SD.AdminEndUser)]

    public class MenuItemsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostingEnvironment;

        [BindProperty]
        public MenuItemViewModel MenuItemVM { get; set; }

        public MenuItemsController(ApplicationDbContext db, IWebHostEnvironment hostingEnvironment)
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

            string webRootPath = _hostingEnvironment.WebRootPath;



            //Image Being Saved
            var files = HttpContext.Request.Form.Files;

            var menuItemFromDb = _db.MenuItems.Find(MenuItemVM.MenuItem.Id);

            //if (files[0] != null && files[0].Length > 0)
            //{
                //when user uploads an image
              //  var uploads = Path.Combine(webRootPath, "Images");
                //var extension = files[0].FileName.Substring(files[0].FileName.LastIndexOf("."), files[0].FileName.Length - files[0].FileName.LastIndexOf("."));
            
                //using (var filestream = new FileStream(Path.Combine(uploads, MenuItemVM.MenuItem.Id + extension), FileMode.Create))
                //{
                  //  files[0].CopyTo(filestream);
                //}
                //menuItemFromDb.Image = @"\Images\" + MenuItemVM.MenuItem.Id + extension;
            //}
            //else
           // {
                //when user does not upload image
             //   var uploads = Path.Combine(webRootPath, @"Images\" + SD.DefaultFoodImage);
               // System.IO.File.Copy(uploads, webRootPath + @"\Images\" + MenuItemVM.MenuItem.Id + ".jpg");
               // menuItemFromDb.Image = @"\Images\" + MenuItemVM.MenuItem.Id + ".jpg";
           // }
            _db.MenuItems.Add(MenuItemVM.MenuItem);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public JsonResult GetSubCategory(int CategoryId)
        {
            List<SubCategory> subCategoryList = new List<SubCategory>();

            subCategoryList = (from subCategory in _db.SubCategory
                               where subCategory.CategoryId == CategoryId
                               select subCategory).ToList();

            return Json(new SelectList(subCategoryList, "Id", "Name"));
        }



        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MenuItemVM.MenuItem = await _db.MenuItems.Include(m => m.Category).Include(m => m.SubCategory).SingleOrDefaultAsync(m => m.Id == id);
            MenuItemVM.SubCategory = _db.SubCategory.Where(s => s.CategoryId == MenuItemVM.MenuItem.CategoryId).ToList();

            if (MenuItemVM.MenuItem == null)
            {
                return NotFound();
            }

            return View(MenuItemVM);
        }

        //POST : Edit MenuItems
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            MenuItemVM.MenuItem.SubCategoryId = Convert.ToInt32(Request.Form["SubCategoryId"].ToString());

            if (id != MenuItemVM.MenuItem.Id)
            {
                return NotFound();
            }

                var menuItemFromDb = _db.MenuItems.Where(m => m.Id == MenuItemVM.MenuItem.Id).FirstOrDefault();
                menuItemFromDb.Name = MenuItemVM.MenuItem.Name;
                menuItemFromDb.Description = MenuItemVM.MenuItem.Description;
                menuItemFromDb.Price = MenuItemVM.MenuItem.Price;
                menuItemFromDb.Spicyness = MenuItemVM.MenuItem.Spicyness;
                menuItemFromDb.CategoryId = MenuItemVM.MenuItem.CategoryId;
                menuItemFromDb.SubCategoryId = MenuItemVM.MenuItem.SubCategoryId;
                await _db.SaveChangesAsync();
            
                MenuItemVM.SubCategory = _db.SubCategory.Where(s => s.CategoryId == MenuItemVM.MenuItem.CategoryId).ToList();
                return RedirectToAction(nameof(Index));


        }

        //GET : Details MenuItem
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MenuItemVM.MenuItem = await _db.MenuItems.Include(m => m.Category).Include(m => m.SubCategory).SingleOrDefaultAsync(m => m.Id == id);

            if (MenuItemVM.MenuItem == null)
            {
                return NotFound();
            }

            return View(MenuItemVM);
        }

        //GET : Delete MenuItem
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MenuItemVM.MenuItem = await _db.MenuItems.Include(m => m.Category).Include(m => m.SubCategory).SingleOrDefaultAsync(m => m.Id == id);

            if (MenuItemVM.MenuItem == null)
            {
                return NotFound();
            }

            return View(MenuItemVM);
        }

        //POST Delete MenuItem
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            MenuItem menuItem = await _db.MenuItems.FindAsync(id);
            _db.MenuItems.Remove(menuItem);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }


}


