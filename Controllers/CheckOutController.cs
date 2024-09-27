using OnlineGasBooking.Models;
//using OnlineGasBooking.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace OnlineGasBooking.Controllers
{
    public class CheckOutController : Controller
    {
        private readonly OnlineGasDBContext _db;

        // Constructor to initialize the database context
        public CheckOutController(OnlineGasDBContext db) => _db = db;
        public ActionResult Index()
        {
            ViewBag.PayMethod = new SelectList(_db.PaymentTypes, "PayTypeID", "TypeName");


            var data = this.GetDefaultData();

            return View(data);
        }


        //PLACE ORDER--LAST STEP

        [HttpPost]
        public ActionResult PlaceOrder(IFormCollection getCheckoutDetails)
        {

            int shpID = 1;
            if (_db.ShippingDetails.Any())
            {
                shpID = _db.ShippingDetails.Max(x => x.ShippingID) + 1;
            }

            int payID = 1;
            if (_db.Payments.Any())
            {
                //payID = _db.Payments.Max(x => x.PaymentID);
                payID = _db.Payments.Max(x => x.PaymentID) + 1;
            }

            int orderID = 1;
            if (_db.Orders.Any())
            {
                orderID = _db.Orders.Max(x => x.OrderID) + 1;
            }
            using (var transaction = _db.Database.BeginTransaction())
            {
                ShippingDetail shpDetails = new ShippingDetail
                {
                    FirstName = getCheckoutDetails["FirstName"],
                    LastName = getCheckoutDetails["LastName"],
                    Email = getCheckoutDetails["Email"],
                    Mobile = getCheckoutDetails["Mobile"],
                    Address = getCheckoutDetails["Address"],
                    City = getCheckoutDetails["City"],
                    PostCode = getCheckoutDetails["PostCode"]
                };
                _db.ShippingDetails.Add(shpDetails);
                _db.SaveChanges();

                Payment pay = new Payment
                {
                    //Type = Convert.ToInt32(getCheckoutDetails["PayMethod"])

                };
                _db.Payments.Add(pay);
                _db.SaveChanges();

                Order o = new Order
                {
                    CustomerID = TempShpData.UserID,
                    PaymentID = payID,
                    ShippingID = shpID,
                    Discount = Convert.ToInt32(getCheckoutDetails["discount"]),
                    TotalAmount = Convert.ToInt32(getCheckoutDetails["totalAmount"]),
                    isCompleted = true,
                    OrderDate = DateTime.Now
                };
                _db.Order.Add(o);
                _db.SaveChanges();

                // Save OrderDetails (if uncommented and used)
                //foreach (var OD in TempShpData.items)
                //{
                //    OD.OrderID = orderID;
                //    OD.Order = _db.Orders.Find(orderID);
                //    OD.Product = _db.Products.Find(OD.ProductID);
                //    _db.OrderDetails.Add(OD);
                //    _db.SaveChanges();
                //}
                transaction.Commit();
            }
            return RedirectToAction("ThankYou", "ThankYou");
        }




        public List<OrderDetail> GetDefaultData()
        {
            if (TempShpData.items == null)
            {
                TempShpData.items = new List<OrderDetail>();
            }
            var data = TempShpData.items.ToList();

            ViewBag.cartBox = data.Count == 0 ? null : data;
            ViewBag.NoOfItem = data.Count();
            int? SubTotal = Convert.ToInt32(data.Sum(x => x.TotalAmount));
            ViewBag.Total = SubTotal;

            int Discount = 0;
            ViewBag.SubTotal = SubTotal;
            ViewBag.Discount = Discount;
            ViewBag.TotalAmount = SubTotal - Discount;

            //ViewBag.WlItemsNo = _db.Wishlists.Where(x => x.CustomerID == TempShpData.UserID).ToList().Count();

            return data;
        }
    }
}
