using OnlineGasBooking.Models;
using Microsoft.AspNetCore.Mvc;

namespace OnlineGasBooking.Controllers
{
    
    
    
    
    public class ThankYouController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.cartBox = null;
            ViewBag.Total = null;
            ViewBag.NoOfItem = null;
            TempShpData.items = null;
            return View();
        }
        public IActionResult ThankYou()
        {
            ViewBag.cartBox = null;
            ViewBag.Total = null;
            ViewBag.NoOfItem = null;
            TempShpData.items = null;
            return View();
        }
    }
}
