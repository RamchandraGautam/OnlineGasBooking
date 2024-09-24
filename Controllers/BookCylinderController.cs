using Microsoft.AspNetCore.Mvc;
using OnlineGasBooking.Models;

namespace OnlineGasBooking.Controllers
{
    public class BookCylinderController : Controller
    {
        private readonly OnlineGasDBContext _db;

        // Constructor to initialize the database context
        public BookCylinderController(OnlineGasDBContext db) => _db = db;

        public IActionResult Index()
        {
            // Fetch only records where status is 'Approve'
            var approvedConnections = _db.NewConnection
                                         .Where(c => c.status == "Approve")
                                         .ToList();
            return View(approvedConnections);
        }

        public IActionResult BookDetails(int id)
        {
            var ConnectionDetail = _db.NewConnection.Find(id);
            if (ConnectionDetail == null)
            {
                return NotFound();
            }

            return View(ConnectionDetail);
        }
        [HttpPost, ActionName("BookDetails")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookingConfirm(int id)
        {
            var ConnectionDetail = _db.NewConnection.Find(id);
            if (ConnectionDetail == null)
            {
                return NotFound();
            }
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
                    FirstName = ConnectionDetail.First_Name,
                    LastName = ConnectionDetail.Last_Name,
                    Email = ConnectionDetail.Email,
                    Mobile = ConnectionDetail.Phone,
                    Address = ConnectionDetail.Address,
                    City = ConnectionDetail.City,
                    PostCode = ConnectionDetail.PostalCode
                };
                _db.ShippingDetails.Add(shpDetails);
                _db.SaveChanges();
                //transaction.Commit();
                Payment pay = new Payment
                {
                    //Type = Convert.ToInt32(1)//getCheckoutDetails["PayMethod"])

                };
                _db.Payments.Add(pay);
                _db.SaveChanges();
                //transaction.Commit();
                Order o = new Order
                {
                    CustomerID = id,//TempShpData.UserID,
                    PaymentID = payID,
                    ShippingID = 4,//shpID,
                    Discount = Convert.ToInt32(0),//getCheckoutDetails["discount"]),
                    TotalAmount = Convert.ToInt32(100),//getCheckoutDetails["totalAmount"]),
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
           // return RedirectToAction("ThankYou", "ThankYou");
            
            //_db.NewConnection.Remove(product);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public IActionResult BookingHistory()
        {
            // Fetch only records where status is 'Approve'
            var approvedConnections = _db.NewConnection
                                         .Where(c => c.status == "Approve")
                                         .ToList();
            return View(approvedConnections);
        }
    }
}
