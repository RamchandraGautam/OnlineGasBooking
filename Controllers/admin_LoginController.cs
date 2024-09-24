using OnlineGasBooking.Models;
using Microsoft.AspNetCore.Mvc;

namespace OnlineGasBooking.Controllers
{
    
    
    
    
    public class admin_LoginController : Controller
    {
        private readonly OnlineGasDBContext _db;

        // Constructor to initialize the database context
        public admin_LoginController(OnlineGasDBContext db) => _db = db;
        // GET: admin_Login
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(admin_Login login)
        {
            //if (ModelState.IsValid)
            //{
                var model = (from m in _db.admin_Login
                             where m.UserName == login.UserName && m.Password == login.Password
                             select m).Any();

                if (model)
                {
                    var loginInfo = _db.admin_Login.Where(x => x.UserName == login.UserName && x.Password == login.Password).FirstOrDefault();

                    HttpContext.Session.SetString("username", loginInfo.UserName);                   
                    TemData.EmpID = loginInfo.EmpID;
                    return RedirectToAction("Index", "Dashboard");
                }
            //}

            return View();
        }
        // Logout Server Code
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();            
            return RedirectToAction("Login", "admin_Login");
        }
    }
}
