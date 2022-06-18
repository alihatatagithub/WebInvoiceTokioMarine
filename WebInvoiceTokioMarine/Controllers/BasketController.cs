using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebInvoiceTokioMarine.Models;
using WebInvoiceTokioMarine.Models.ViewModels;

namespace WebInvoiceTokioMarine.Controllers
{
    [Authorize]
    public class BasketController : Controller
    {
        private static List<AddToCartGetViewModel> _basketRepo =
                                    new List<AddToCartGetViewModel>();
        UserManager<ApplicationUser> _userManager = new UserManager<ApplicationUser>
                (new UserStore<ApplicationUser>(new ApplicationDbContext()));

        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        // GET: Basket
        public ActionResult AddToCart(int? id)
        {
            var product = _db.Products.FirstOrDefault(a => a.Id == id);
            var category = _db.Products.FirstOrDefault(a => a.CategoryId == product.CategoryId);
            var CustomerId = User.Identity.GetUserId();
            var CartViewModel = new AddToCartGetViewModel()
            {
                ProductId = product.Id,
                CategoryName = category.Name,
                ProductDescription = product.Description,
                ProductName = product.Name,
                PictureUrl = product.ImageUrl,
                CustomerId = CustomerId,
                Price = product.Price,
                Total = product.Price,
                SupplierId = product.SupplierId
            };
            return PartialView(CartViewModel);
        }
        [HttpPost]
        public ActionResult AddToCart(AddToCartGetViewModel model)
        {
            var CustomerId = User.Identity.GetUserId();
            var product = _db.Products.FirstOrDefault(a => a.Id == model.ProductId);
            var category = _db.Products.FirstOrDefault(a => a.CategoryId == product.CategoryId);
            if (!_basketRepo.Any(a => a.ProductId == model.ProductId && a.CustomerId == CustomerId))
            {
                var NewAddCart = new AddToCartGetViewModel()
                {
                    ProductId = model.ProductId,
                    Qty = model.Qty,
                    Total = product.Price * model.Qty,
                    CustomerId = CustomerId,
                    CategoryName = category.Name,
                    PictureUrl= product.ImageUrl,
                    Price = product.Price,
                    ProductName = product.Name,
                    CreateDate = DateTime.UtcNow,
                    ProductDescription= product.Description,

                };
                _basketRepo.Add(NewAddCart);
            }
            else
            {
                var OldProductCart = _basketRepo.FirstOrDefault
                    (a => a.ProductId == model.ProductId && a.CustomerId == CustomerId);
                OldProductCart.Qty += model.Qty;
                OldProductCart.CreateDate = DateTime.UtcNow;
                OldProductCart.Total = product.Price * OldProductCart.Qty;
            }

            return RedirectToAction("ViewCart");

        }


        public ActionResult ViewCart()
        {
            var customerId = User.Identity.GetUserId();
            var Repo = _basketRepo.Where(a => a.CustomerId == customerId );

            return View(Repo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Confirm()
        {
            var customerId = User.Identity.GetUserId();
            var customer = await _userManager.FindByIdAsync(customerId);
         
            var Repo = _basketRepo.Where(a => a.CustomerId == customerId);
            var NewOrder = new Order()
            {
                CreatedDate = DateTime.UtcNow,
                CreatedBy = customerId
            };
            _db.Orders.Add(NewOrder);
            _db.SaveChanges();
            if (Repo.Count() > 0)
            {
                foreach (var item in Repo)
                {
                    OrderDetail NewOrderDetail = new OrderDetail();
                    NewOrderDetail.OrderId = NewOrder.OrderId;
                    NewOrderDetail.ProductId = item.ProductId;
                    NewOrderDetail.CreatedDate = item.CreateDate;
                    NewOrderDetail.Qty = item.Qty;
                    NewOrderDetail.CustomerId = customerId;
                    NewOrderDetail.Total = item.Total;
                    NewOrderDetail.SupplierId = item.SupplierId;

                    NewOrder.TotalOrder += item.Total;

                    _db.OrderDetails.Add(NewOrderDetail);

                }
               
                _db.SaveChanges();
            }
            var callbackUrl = Url.Action("PrintInvoice", "Basket", new { userId = customerId,orderId = NewOrder.OrderId }, protocol: Request.Url.Scheme);
            IdentityMessage message = new IdentityMessage
            {
                Body = callbackUrl,
                Subject = "Congratulation you have complete your order Successfully",
                Destination = customer.Email
            };
            EmailService email = new EmailService();
            await email.SendAsync(message);

            if (_basketRepo.Any())
            {
                _basketRepo.Clear();
            }
            return RedirectToAction("Index","Orders");



        }

        public ActionResult PrintInvoice(string userId,int? orderId)
        {
            var InvoiceData = _db.OrderDetails.Where(a => a.CustomerId == userId && a.OrderId == orderId)
                .Include(a => a.Customer).Include(a => a.Product).Include(a => a.Supplier).Include(a => a.Order);

            return new PartialViewAsPdf("_Invoice", InvoiceData) { FileName = "Invoice.pdf" };
        }

    }
}