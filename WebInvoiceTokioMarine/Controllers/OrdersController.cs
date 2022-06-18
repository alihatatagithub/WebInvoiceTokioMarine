using Microsoft.AspNet.Identity;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebInvoiceTokioMarine.Models;

namespace WebInvoiceTokioMarine.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        public OrdersController()
        {
        }
        [HttpGet]
        public ActionResult Index()
        {
            var customerId = User.Identity.GetUserId();
            var orders = _db.Orders.Where(a => a.CreatedBy == customerId)
                .Include(a => a.OrderDetails).Include(a => a.Customer);

            return View(orders);
        }

  
        [HttpGet]
        public ActionResult Details(int? id)
        {
            var customerId = User.Identity.GetUserId();
            var orderDetails = _db.OrderDetails.Where(a => a.CustomerId == customerId && a.OrderId == id)
                .Include(a => a.Order).Include(a => a.Product).Include(a => a.Supplier);
            return View(orderDetails);

        }
    }
}