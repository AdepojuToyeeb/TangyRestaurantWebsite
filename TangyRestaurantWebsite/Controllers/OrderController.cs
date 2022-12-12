using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TangyRestaurantWebsite.Data;
using TangyRestaurantWebsite.Models;
using TangyRestaurantWebsite.Models.OrderDetailsViewModel;
using TangyRestaurantWebsite.Utility;

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

        [Authorize]
        public IActionResult OrderHistory()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            List<OrderDetailsViewModel> OrderDetailsVM = new List<OrderDetailsViewModel>();

            List<OrderHeader> OrderHeaderList = _db.OrderHeader.Where(u => u.UserId == claim.Value).OrderByDescending(u => u.OrderDate).ToList();

            foreach (OrderHeader item in OrderHeaderList)
            {
                OrderDetailsViewModel individual = new OrderDetailsViewModel();
                individual.OrderHeader = item;
                individual.OrderDetail = _db.OrderDetail.Where(o => o.OrderId == item.Id).ToList();
                OrderDetailsVM.Add(individual);

            }
            return View(OrderDetailsVM);
        }

        [Authorize(Roles = SD.AdminEndUser)]
        public IActionResult ManageOrder()
        {
            List<OrderDetailsViewModel> OrderDetailsVM = new List<OrderDetailsViewModel>();
            List<OrderHeader> OrderHeaderList = _db.OrderHeader.Where(o => o.Status == SD.StatusSubmitted || o.Status == SD.StatusInProcess)
                .OrderByDescending(u => u.PickUpTime).ToList();

            foreach (OrderHeader item in OrderHeaderList)
            {
                OrderDetailsViewModel individual = new OrderDetailsViewModel();
                individual.OrderHeader = item;
                individual.OrderDetail = _db.OrderDetail.Where(o => o.OrderId == item.Id).ToList();
                OrderDetailsVM.Add(individual);

            }
            return View(OrderDetailsVM);
        }

        [Authorize(Roles = SD.AdminEndUser)]
        public async Task<IActionResult> OrderPrepare(int orderId)
        {
            OrderHeader orderHeader = _db.OrderHeader.Find(orderId);
            orderHeader.Status = SD.StatusInProcess;
            await _db.SaveChangesAsync();
            return RedirectToAction("ManageOrder", "Order");

        }

        [Authorize(Roles = SD.AdminEndUser)]
        public async Task<IActionResult> OrderReady(int orderId)
        {
            OrderHeader orderHeader = _db.OrderHeader.Find(orderId);
            orderHeader.Status = SD.StatusReady;
            await _db.SaveChangesAsync();
            return RedirectToAction("ManageOrder", "Order");

        }

        [Authorize(Roles = SD.AdminEndUser)]
        public async Task<IActionResult> OrderCancel(int orderId)
        {
            OrderHeader orderHeader = _db.OrderHeader.Find(orderId);
            orderHeader.Status = SD.StatusCancelled;
            await _db.SaveChangesAsync();
            return RedirectToAction("ManageOrder", "Order");

        }

        public IActionResult OrderPickup()
        {
            List<OrderDetailsViewModel> OrderDetailsVM = new List<OrderDetailsViewModel>();
            List<OrderHeader> OrderHeaderList = _db.OrderHeader.Where(o => o.Status == SD.StatusReady)
                .OrderByDescending(u => u.PickUpTime).ToList();

            foreach (OrderHeader item in OrderHeaderList)
            {
                OrderDetailsViewModel individual = new OrderDetailsViewModel();
                individual.OrderHeader = item;
                individual.OrderDetail = _db.OrderDetail.Where(o => o.OrderId == item.Id).ToList();
                OrderDetailsVM.Add(individual);

            }
            return View(OrderDetailsVM);
        }

    }
}

