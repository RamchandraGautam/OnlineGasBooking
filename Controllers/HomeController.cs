using Microsoft.AspNetCore.Mvc;
using OnlineGasBooking.Models;
using System.Diagnostics;

namespace OnlineGasBooking.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        private readonly OnlineGasDBContext _db;

        // Constructor to initialize the database context
        public HomeController(OnlineGasDBContext db) => _db = db;
        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public IActionResult Index()
        {
            ViewBag.Mobile = _db.Products.Where(x => x.Category.Name.Equals("Mobile")).ToList();
            ViewBag.Laptop = _db.Products.Where(x => x.Category.Name.Equals("Laptop")).ToList();
            ViewBag.Speaker = _db.Products.Where(x => x.Category.Name.Equals("Speakers")).ToList();
            ViewBag.ElectronicsProduct = _db.Products.Where(x => x.Category.Name.Equals("Electronic Product")).ToList();
            ViewBag.Slider = _db.genMainSliders.ToList();
            ViewBag.PromoRight = _db.genPromoRights.ToList();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
