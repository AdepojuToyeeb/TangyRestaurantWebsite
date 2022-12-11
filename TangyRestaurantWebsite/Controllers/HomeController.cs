using System.Diagnostics;
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

    public async Task<IActionResult> Details (int id)
    {
        var MenuItemFromDb = await _db.MenuItems.Include(m => m.Category).Include(m => m.SubCategory).Where(m => m.Id == id).FirstOrDefaultAsync();
        ShoppingCart CartObj = new ShoppingCart()
        {
            MenuItem = MenuItemFromDb,
            MenuItemId = MenuItemFromDb.Id
        };
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

