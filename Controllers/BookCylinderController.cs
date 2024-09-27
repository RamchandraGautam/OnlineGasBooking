using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OnlineGasBooking.Models;
using System;

namespace OnlineGasBooking.Controllers
{
    public class BookCylinderController : Controller
    {
        private readonly OnlineGasDBContext _db;

        // Constructor to initialize the database context
        public BookCylinderController(OnlineGasDBContext db) => _db = db;

        public IActionResult Index()
        {
           
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
        public async Task<IActionResult> BookingConfirm(int id, string AmountToPaid, string SelectedPaymentType)
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

                // Handle payment type logic here
                PaymentType paymentType = null;

                switch (SelectedPaymentType)
                {
                    case "Credit Card":
                        paymentType = _db.PaymentTypes.FirstOrDefault(p => p.TypeName == "Credit Card");
                        break;
                    case "Net Banking":
                        paymentType = _db.PaymentTypes.FirstOrDefault(p => p.TypeName == "Net Banking");
                        break;
                    case "UPI":
                        paymentType = _db.PaymentTypes.FirstOrDefault(p => p.TypeName == "UPI");
                        break;
                    case "Cash on Delivery":
                        paymentType = _db.PaymentTypes.FirstOrDefault(p => p.TypeName == "Cash on Delivery");
                        break;
                    default:
                        ModelState.AddModelError("", "Please select a valid payment type.");
                        return View(ConnectionDetail);
                }

                if (paymentType == null)
                {
                    ModelState.AddModelError("", "Selected payment type is invalid.");
                    return View(ConnectionDetail);
                }

                Payment pay = new Payment
                {
                    PaymentStatus = "Success",
                    Amount = Convert.ToDecimal(AmountToPaid),
                    PaymentDateTime = DateTime.Now,
                    PaymentType = paymentType // Set payment type here
                };

                _db.Payments.Add(pay);
                _db.SaveChanges();

                Order o = new Order
                {
                    CustomerID = Convert.ToInt32(HttpContext.Session.GetInt32("CustomerID")),
                    PaymentID = payID,
                    ShippingID = shpID,
                    Discount = id,
                    Taxes = 0,
                    TotalAmount = Convert.ToInt32(AmountToPaid),
                    isCompleted = true,
                    ShippingDate= DateTime.Now.AddDays(2),
                    OrderDate = DateTime.Now
                };

                _db.Orders.Add(o);
                _db.SaveChanges();

                transaction.Commit();
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(BookingHistory));
        }


        public IActionResult BookingHistory()
        {
            // Assuming you have a customerId to pass
           
            int customerId =Convert.ToInt32(HttpContext.Session.GetInt32("CustomerID"));
            string? connectionString = _db.Database.GetConnectionString(); // Retrieve your connection string
            using (var connection = new SqlConnection(connectionString))
            {
                // Raw SQL query without mapping to a model
                var query = "SELECT OrderID,FirstName,LastName,Email,Mobile,PaymentStatus,isCompleted,OrderDate,ShippingDate,Shipped,Address,Notes FROM ViewOrderDetails WHERE CustomerID = @CustomerId";// 

                // Execute the query and retrieve the data as a list of dynamic objects
                var result = connection.Query(query, new { CustomerId = customerId }).ToList();//


                ViewBag.BookHistory = result;
                return View();
            }
        }
    }
}
