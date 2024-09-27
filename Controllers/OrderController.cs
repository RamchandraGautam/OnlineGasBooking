using OnlineGasBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Dapper;

namespace OnlineGasBooking.Controllers
{
   
    public class OrderController : Controller
    {
        private readonly OnlineGasDBContext _db;

        // Constructor to initialize the database context
        public OrderController(OnlineGasDBContext db) => _db = db;
        public ActionResult Index()
        {
            //int customerId = Convert.ToInt32(HttpContext.Session.GetInt32("CustomerID"));
            string? connectionString = _db.Database.GetConnectionString(); // Retrieve your connection string
            using (var connection = new SqlConnection(connectionString))
            {
                // Raw SQL query without mapping to a model
                var query = "SELECT OrderID,FirstName,LastName,Email,Mobile,PaymentStatus,isCompleted,OrderDate,ShippingDate,Shipped,Address,Notes FROM ViewOrderDetails ";// WHERE CustomerID = @CustomerId

                // Execute the query and retrieve the data as a list of dynamic objects
                var result = connection.Query(query).ToList();//, new { CustomerId = customerId }


                ViewBag.BookHistory = result;
                return View();
            }            
        }
        public ActionResult Details(int id)
        {
            string? connectionString = _db.Database.GetConnectionString(); // Retrieve your connection string
            using (var connection = new SqlConnection(connectionString))
            {
                // Raw SQL query without mapping to a model
                var query = "SELECT * FROM [Order] where OrderID=4 ";// WHERE CustomerID = @CustomerId

                // Execute the query and retrieve the data as a list of dynamic objects
                var result = connection.Query(query);//, new { CustomerId = customerId }


                //ViewBag.BookHistory = result;
                return View();
            }
            //BookingDetails ord = _db.Orders.Find(id);
            //var Ord_details = _db.OrderDetails.Where(x => x.OrderID == id);
            ////var tuple = new Tuple<Order, IEnumerable<OrderDetail>>(ord, Ord_details);

            ////double SumAmount = Convert.ToDouble(Ord_details.Sum(x => x.TotalAmount));
            ////ViewBag.TotalItems = Ord_details.Sum(x => x.Quantity);
            ////ViewBag.Discount = 0;
            ////ViewBag.TAmount = SumAmount - 0;
            ////ViewBag.Amount = SumAmount;
            //return View(ord);//tuple
        }
    }
}
