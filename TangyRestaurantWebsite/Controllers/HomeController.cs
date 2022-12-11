using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TangyRestaurantWebsite.Data;
using TangyRestaurantWebsite.Models;
using TangyRestaurantWebsite.Models.HomeViewModel;

namespace TangyRestaurantWebsite.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _db;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        IndexViewModel IndexVM = new IndexViewModel()
        {
            MenuItem = await _db.MenuItems.Include(m => m.Category).Include(m => m.SubCategory).ToListAsync(),
            Category = _db.Category.OrderBy(c=>c.DisplayOrder),
            Coupons = _db.Coupons.Where(c=>c.isActive==true).ToList()
        };
        return View(IndexVM);
    }

    [Authorize]
    public async Task<IActionResult> Details (int id)
    {
        var MenuItemFromDb = await _db.MenuItems.Include(m => m.Category).Include(m => m.SubCategory).Where(m => m.Id == id).FirstOrDefaultAsync();
        ShoppingCart CartObj = new ShoppingCart()
        {
            MenuItem = MenuItemFromDb,
            MenuItemId = MenuItemFromDb.Id
        };
        return View(CartObj);
    }
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Details (ShoppingCart CartObject)
    {
        CartObject.Id = 0;
        var errors = ModelState.Values.SelectMany(v => v.Errors);

        if (ModelState.IsValid)
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            CartObject.ApplicationUserId = claim.Value;

            ShoppingCart cartFromDb = await _db.ShoppingCart.Where(c => c.ApplicationUserId == CartObject.ApplicationUserId && c.MenuItemId == CartObject.MenuItemId).FirstOrDefaultAsync();

            if(cartFromDb == null)
            {
                //this menu items doesnt exist
                _db.ShoppingCart.Add(CartObject);
            }
            else
            {
                //menu items exists in shopping cart for that user so just update the count
                cartFromDb.Count = cartFromDb.Count + CartObject.Count;
            }

            await _db.SaveChangesAsync();

            var count = _db.ShoppingCart.Where(c => c.ApplicationUserId == CartObject.ApplicationUserId).ToList().Count;
            HttpContext.Session.SetInt32("CartCount", count);

            return RedirectToAction(nameof(Index));
        }

        else
        {
            var MenuItemFromDb = await _db.MenuItems.Include(m => m.Category).Include(m => m.SubCategory).Where(m => m.Id == CartObject.MenuItemId).FirstOrDefaultAsync();
            ShoppingCart CartObj = new ShoppingCart()
            {
                MenuItem = MenuItemFromDb,
                MenuItemId = MenuItemFromDb.Id
            };
            return View(CartObj);
        }
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

