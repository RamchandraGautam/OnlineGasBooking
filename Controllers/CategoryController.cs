using OnlineGasBooking.Models;
using Microsoft.AspNetCore.Mvc;

namespace OnlineGasBooking.Controllers
{
    public class CategoryController : Controller
    {
        
        
        
        
        private readonly OnlineGasDBContext _db;

        // Constructor to initialize the database context
        public CategoryController(OnlineGasDBContext db) => _db = db;
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category ctg)
        {
            if (ModelState.IsValid)
            {

                _db.Categories.Add(ctg);
                _db.SaveChanges();
                return PartialView("_Success");
            }
            return PartialView("Error");
           

        }
    }
}
