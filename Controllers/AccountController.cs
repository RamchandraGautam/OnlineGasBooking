using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineGasBooking.Models;

namespace OnlineGasBooking.Controllers
{
    public class AccountController : Controller
    {
        private readonly OnlineGasDBContext _db;

        public AccountController(OnlineGasDBContext db) => _db = db;

        // GET: Account/Index
        public IActionResult Index()
        {
            HttpContext.Session.SetString("username", "");
            var usr = _db.Customers.Find(TempShpData.UserID);
            return View(usr);
        }

        // GET: Account/RegisterCustomer
        public IActionResult RegisterCustomer()
        {
            return View();
        }

        // POST: Account/RegisterCustomer
        [HttpPost]
        public IActionResult RegisterCustomer(CustomerVM cvm)
        {
            if (!ModelState.IsValid)
            {
                return View(cvm);
            }

            try
            {
                if (cvm.Picture != null)
                {
                    // Handle file upload
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", Guid.NewGuid() + Path.GetExtension(cvm.Picture.FileName));

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        cvm.Picture.CopyTo(stream);
                    }

                    var customer = new Customer
                    {
                        CustomerID = cvm.CustomerID,
                        First_Name = cvm.First_Name,
                        Last_Name = cvm.Last_Name,
                        UserName = cvm.UserName,
                        Password = cvm.Password,
                        Gender = cvm.Gender,
                        DateofBirth = cvm.DateofBirth,
                        Country = cvm.Country,
                        City = cvm.City,
                        PostalCode = cvm.PostalCode,
                        Email = cvm.Email,
                        Phone = cvm.Phone,
                        Address = cvm.Address,
                        PicturePath = "/Images/" + Path.GetFileName(filePath),
                        status = cvm.status,
                        LastLogin = cvm.LastLogin,
                        Created = cvm.Created,
                        Notes = cvm.Notes
                    };

                    _db.Customers.Add(customer);
                    _db.SaveChanges();

                    return RedirectToAction("Login");
                }
            }
            catch (Exception ex)
            {
                // Log the exception and show error view
                // _logger.LogError(ex, "Error while registering customer.");
                return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
            }

            return View(cvm);
        }

        // GET: Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        public IActionResult Login(string userName, string password)
        {
            if (ModelState.IsValid)
            {
                var customer = _db.Customers.SingleOrDefault(c => c.UserName == userName && c.Password == password);

                if (customer != null)
                {
                    TempShpData.UserID = customer.CustomerID;
                    HttpContext.Session.SetString("username", customer.UserName);
                    HttpContext.Session.SetInt32("CustomerID", customer.CustomerID);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Invalid login attempt.");
            }

            return View();
        }

        // GET: Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("username");
            TempShpData.UserID = 0;
            TempShpData.items = null;
            return RedirectToAction("Index", "Home");
        }

        private Customer GetUser(string userName)
        {
            return _db.Customers.SingleOrDefault(c => c.UserName == userName);
        }

        // POST: Account/Update
        [HttpPost]
        public IActionResult Update(Customer customer)
        {
            if (ModelState.IsValid)
            {
                _db.Customers.Update(customer);
                _db.SaveChanges();
                HttpContext.Session.SetString("username", customer.UserName);

                return RedirectToAction("Index", "Home");
            }

            return View(customer);
        }
    }
}
