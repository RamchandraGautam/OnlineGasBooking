using OnlineGasBooking.Models;
using Microsoft.AspNetCore.Mvc;

namespace OnlineGasBooking.Controllers
{
    public class ProfileController : Controller
    {
        private readonly OnlineGasDBContext _db;

        // Constructor to initialize the database context
        public ProfileController(OnlineGasDBContext db) => _db = db;
        public ActionResult Index()
        {
            return View(_db.admin_Employee.Find(TemData.EmpID));
        }
    }
}
