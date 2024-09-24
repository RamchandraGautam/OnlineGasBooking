using OnlineGasBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineGasBooking.Controllers
{
    
    
    
    
    public class CustomerController : Controller
    {
        private readonly OnlineGasDBContext _db;
        private readonly IWebHostEnvironment _hostingEnvironment;

        // Constructor to initialize the database context and hosting environment
        public CustomerController(OnlineGasDBContext db, IWebHostEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Customer
        public IActionResult Index()
        {
            return View(_db.Customers.ToList());
        }

        // GET: Customer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerVM cvm)
        {
            if (ModelState.IsValid)
            {
                string filePath = null;
                if (cvm.Picture != null)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(cvm.Picture.FileName);
                    var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "Images");
                    filePath = Path.Combine("Images", fileName);
                    var filePathFull = Path.Combine(uploads, fileName);

                    using (var stream = new FileStream(filePathFull, FileMode.Create))
                    {
                        await cvm.Picture.CopyToAsync(stream);
                    }
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
                    PicturePath = filePath,
                    status = cvm.status,
                    LastLogin = cvm.LastLogin,
                    Created = cvm.Created,
                    Notes = cvm.Notes
                };

                _db.Customers.Add(customer);
                await _db.SaveChangesAsync();

                return PartialView("_Success");
            }

            return PartialView("_Error");
        }

        // GET: Customer/Edit/5
        public IActionResult Edit(int id)
        {
            var customer = _db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            var cvm = new CustomerVM
            {
                CustomerID = customer.CustomerID,
                First_Name = customer.First_Name,
                Last_Name = customer.Last_Name,
                UserName = customer.UserName,
                Password = customer.Password,
                Gender = customer.Gender,
                DateofBirth = customer.DateofBirth,
                Country = customer.Country,
                City = customer.City,
                PostalCode = customer.PostalCode,
                Email = customer.Email,
                Phone = customer.Phone,
                Address = customer.Address,
                PicturePath = customer.PicturePath,
                status = customer.status,
                LastLogin = customer.LastLogin,
                Created = customer.Created,
                Notes = customer.Notes
            };

            return View(cvm);
        }

        // POST: Customer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CustomerVM cvm)
        {
            if (ModelState.IsValid)
            {
                string filePath = cvm.PicturePath;
                if (cvm.Picture != null)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(cvm.Picture.FileName);
                    var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "Images");
                    filePath = Path.Combine("Images", fileName);
                    var filePathFull = Path.Combine(uploads, fileName);

                    using (var stream = new FileStream(filePathFull, FileMode.Create))
                    {
                        await cvm.Picture.CopyToAsync(stream);
                    }
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
                    PicturePath = filePath,
                    status = cvm.status,
                    LastLogin = cvm.LastLogin,
                    Created = cvm.Created,
                    Notes = cvm.Notes
                };

                _db.Entry(customer).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(cvm);
        }

        // GET: Customer/Details/5
        public IActionResult Details(int id)
        {
            var customer = _db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            var cvm = new CustomerVM
            {
                CustomerID = customer.CustomerID,
                First_Name = customer.First_Name,
                Last_Name = customer.Last_Name,
                UserName = customer.UserName,
                Password = customer.Password,
                Gender = customer.Gender,
                DateofBirth = customer.DateofBirth,
                Country = customer.Country,
                City = customer.City,
                PostalCode = customer.PostalCode,
                Email = customer.Email,
                Phone = customer.Phone,
                Address = customer.Address,
                PicturePath = customer.PicturePath,
                status = customer.status,
                LastLogin = customer.LastLogin,
                Created = customer.Created,
                Notes = customer.Notes
            };

            return View(cvm);
        }

        // GET: Customer/Delete/5
        public IActionResult Delete(int id)
        {
            var customer = _db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = _db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, customer.PicturePath);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _db.Customers.Remove(customer);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
