using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TangyRestaurantWebsite.Data;
using TangyRestaurantWebsite.Models;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TangyRestaurantWebsite.Controllers
{
    public class CouponsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CouponsController(ApplicationDbContext db)
        {
            _db = db;
        }



        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            return View(await _db.Coupons.ToListAsync());
        }

        //GET Action method for create
        public IActionResult Create()
        {
            return View();
        }

        //POST action method for Create Action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Coupons coupons)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            var files = HttpContext.Request.Form.Files;

            if (ModelState.IsValid)
            {
                //var files = HttpContext.Request.Form.Files;
                if (files[0] != null && files[0].Length > 0)
                {
                    byte[] p1 = null;
                    using (var fs1 = files[0].OpenReadStream())
                    {
                        using (var ms1 = new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1 = ms1.ToArray();
                        }
                    }
                    coupons.Picture = p1;
                    _db.Coupons.Add(coupons);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                
            }
            return View(coupons);
        }

        //GET method for Edit
        public async Task<IActionResult>Edit(int? Id)
        {
            if(Id == null)
            {
                return NotFound();
            }
            var coupon = await _db.Coupons.SingleOrDefaultAsync(m => m.Id == Id);
            if(coupon == null)
            {
                return NotFound();
            }
            return View(coupon);
        }

        //POST method for Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Edit(int Id, Coupons coupons)
        {
            if (Id != coupons.Id)
            {
                return NotFound();
            }

            var couponFromDb = await _db.Coupons.Where(c => c.Id == Id).FirstOrDefaultAsync();
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                var errors = ModelState.Values.SelectMany(v => v.Errors);

                if (files[0] != null && files[0].Length > 0)
                {
                    byte[] p1 = null;
                    using (var fs1 = files[0].OpenReadStream())
                    {
                        using (var ms1 = new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1 = ms1.ToArray();
                        }
                    }
                    couponFromDb.Picture = p1;
                }
                couponFromDb.MinimumAmount = coupons.MinimumAmount;
                couponFromDb.Name = coupons.Name;
                couponFromDb.Discount = coupons.Discount;
                couponFromDb.CouponType = coupons.CouponType;
                couponFromDb.isActive = coupons.isActive;

                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(coupons);
        }
    }
}

