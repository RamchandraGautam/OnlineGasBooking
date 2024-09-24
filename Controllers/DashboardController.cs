using OnlineGasBooking.Models;
using Microsoft.AspNetCore.Mvc;

namespace OnlineGasBooking.Controllers
{
    
    
    
    
    public class DashboardController : Controller
    {
        private readonly OnlineGasDBContext _db;

        // Constructor to initialize the database context
        public DashboardController(OnlineGasDBContext db) => _db = db;
        public IActionResult Index()
        {
            ViewBag.latestOrders = _db.NewConnection.OrderByDescending(x => x.NewConnectionID).Take(10).ToList();
            //ViewBag.latestOrders = _db.Orders.OrderByDescending(x => x.OrderID).Take(10).ToList();
            //ViewBag.NewOrders = db.Orders.Where(a => a.DIspatched == false && a.Shipped == false && a.Deliver == false).Count();
            //ViewBag.DispatchedOrders = db.Orders.Where(a => a.DIspatched == true && a.Shipped == false && a.Deliver == false).Count();
            //ViewBag.ShippedOrders = db.Orders.Where(a => a.DIspatched == true && a.Shipped == true && a.Deliver == false).Count();
            //ViewBag.latestOrders = db.Orders.Where(a => a.DIspatched == true && a.Shipped == true && a.Deliver == true).Count();
            return View();
        }
    }
}
