using OnlineGasBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Dapper;

namespace OnlineGasBooking.Controllers
{
    
    
    
    
    public class DashboardController : Controller
    {
        private readonly OnlineGasDBContext _db;

        // Constructor to initialize the database context
        public DashboardController(OnlineGasDBContext db) => _db = db;
        public IActionResult Index()
        {
            ViewBag.Messege = "Welcome to Admin Portal";
            DashBoardCounts dashboardCounts = new DashBoardCounts();
            string? connectionString = _db.Database.GetConnectionString();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM ViewDashBoardCounts";

                var result = db.QueryFirstOrDefault<DashBoardCounts>(query);

                if (result != null)
                {
                    dashboardCounts = result;
                }
            }
            return View(dashboardCounts);           
        }
    }
}
