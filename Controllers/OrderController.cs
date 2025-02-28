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
                var query = "SELECT OrderID,FirstName,LastName,Email,Mobile,PaymentStatus,TotalAmount,isCompleted,OrderDate,ShippingDate,Address,Notes FROM ViewOrderDetails ";// WHERE CustomerID = @CustomerId

                // Execute the query and retrieve the data as a list of dynamic objects
                var result = connection.Query(query).ToList();//, new { CustomerId = customerId }


                ViewBag.BookHistory = result;
                return View();
            }            
        }
        public ActionResult Details(int id)
        {
            string? connectionString = _db.Database.GetConnectionString(); 
            using (var connection = new SqlConnection(connectionString))
            {
                // Raw SQL query mapped to the BookingDetails model
                var query = "SELECT OrderID,PaymentID,FirstName,LastName,Email,Mobile,PaymentStatus,isCompleted,OrderDate,ShippingDate,Address,Notes FROM ViewOrderDetails WHERE OrderID = @OrderID";

                // Execute the query and map the result to BookingDetails model
                var result = connection.QueryFirstOrDefault<BookingDetails>(query, new { OrderID = id });

                if (result == null)
                {
                    //return HttpNotFound();
                }
                return View(result);
            }           
        }
        [HttpPost]
        public async Task<IActionResult> UpdateOrder(BookingDetails orderData)
        {
            try
            {
                int orderId = orderData.OrderID;
                bool? isCompleted = orderData.isCompleted;
                string? note = orderData.Notes;
                DateTime? deliverydate=orderData.ShippingDate;


                string? connectionString = _db.Database.GetConnectionString();
                using (var connection = new SqlConnection(connectionString))
                {
                    var query = "UPDATE [Order] SET isCompleted = @IsCompleted, Notes = @Note, ShippingDate=@deliverydate WHERE OrderID = @OrderID";

                    // Parameterized query to prevent SQL injection
                    var result = await connection.ExecuteAsync(query, new
                    {
                        IsCompleted = isCompleted,
                        Note = note,
                        DeliveryDate = deliverydate,
                        OrderID = orderId
                    });

                    if (result <= 0) // If no rows were updated
                    {
                        return PartialView("_Error");
                    }
                }

                return PartialView("_Success");
            }
            catch (Exception ex)
            {
                // Log the exception (optional) and return an error partial view
                return PartialView("_Error");
            }
        }

    }
}
