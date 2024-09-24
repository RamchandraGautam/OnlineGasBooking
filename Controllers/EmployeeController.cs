using OnlineGasBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineGasBooking.Controllers
{
    
    
    
    
    public class EmployeeController : Controller
    {
        private readonly OnlineGasDBContext _db;
        private readonly IWebHostEnvironment _hostingEnvironment;

        // Constructor to initialize the database context and hosting environment
        public EmployeeController(OnlineGasDBContext db, IWebHostEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Employee
        public IActionResult Index()
        {
            return View(_db.admin_Employee.ToList());
        }

        // GET: Employee/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeVM evm)
        {
            if (ModelState.IsValid)
            {
                string filePath = null;
                if (evm.Picture != null)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(evm.Picture.FileName);
                    var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "Images");
                    filePath = Path.Combine("Images", fileName);
                    var filePathFull = Path.Combine(uploads, fileName);

                    using (var stream = new FileStream(filePathFull, FileMode.Create))
                    {
                        await evm.Picture.CopyToAsync(stream);
                    }
                }

                var employee = new admin_Employee
                {
                    EmpID = evm.EmpID,
                    FirstName = evm.FirstName,
                    LastName = evm.LastName,
                    DateofBirth = evm.DateofBirth,
                    Gender = evm.Gender,
                    Email = evm.Email,
                    Address = evm.Address,
                    Phone = evm.Phone,
                    PicturePath = filePath
                };

                _db.admin_Employee.Add(employee);
                await _db.SaveChangesAsync();

                return PartialView("_Success");
            }

            return PartialView("_Error");
        }

        // GET: Employee/Edit/5
        public IActionResult Edit(int id)
        {
            var employee = _db.admin_Employee.Find(id);
            if (employee == null)
            {
                return NotFound();
            }

            var evm = new EmployeeVM
            {
                EmpID = employee.EmpID,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                DateofBirth = employee.DateofBirth,
                Gender = employee.Gender,
                Email = employee.Email,
                Address = employee.Address,
                Phone = employee.Phone,
                PicturePath = employee.PicturePath
            };

            return View(evm);
        }

        // POST: Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeVM evm)
        {
            if (ModelState.IsValid)
            {
                var employee = _db.admin_Employee.Find(evm.EmpID);
                if (employee == null)
                {
                    return NotFound();
                }

                string filePath = evm.PicturePath;
                if (evm.Picture != null)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(evm.Picture.FileName);
                    var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "Images");
                    filePath = Path.Combine("Images", fileName);
                    var filePathFull = Path.Combine(uploads, fileName);

                    using (var stream = new FileStream(filePathFull, FileMode.Create))
                    {
                        await evm.Picture.CopyToAsync(stream);
                    }
                }

                employee.FirstName = evm.FirstName;
                employee.LastName = evm.LastName;
                employee.DateofBirth = evm.DateofBirth;
                employee.Gender = evm.Gender;
                employee.Email = evm.Email;
                employee.Address = evm.Address;
                employee.Phone = evm.Phone;
                employee.PicturePath = filePath;

                _db.Entry(employee).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(evm);
        }

        // GET: Employee/Info/5
        public IActionResult Info(int id)
        {
            var employee = _db.admin_Employee.Find(id);
            if (employee == null)
            {
                return NotFound();
            }

            var evm = new EmployeeVM
            {
                EmpID = employee.EmpID,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                DateofBirth = employee.DateofBirth,
                Gender = employee.Gender,
                Email = employee.Email,
                Address = employee.Address,
                Phone = employee.Phone,
                PicturePath = employee.PicturePath
            };

            return View(evm);
        }

        // POST: Employee/Info/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Info(EmployeeVM evm)
        {
            if (ModelState.IsValid)
            {
                var employee = _db.admin_Employee.Find(evm.EmpID);
                if (employee == null)
                {
                    return NotFound();
                }

                string filePath = evm.PicturePath;
                if (evm.Picture != null)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(evm.Picture.FileName);
                    var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "Images");
                    filePath = Path.Combine("Images", fileName);
                    var filePathFull = Path.Combine(uploads, fileName);

                    using (var stream = new FileStream(filePathFull, FileMode.Create))
                    {
                        await evm.Picture.CopyToAsync(stream);
                    }
                }

                employee.FirstName = evm.FirstName;
                employee.LastName = evm.LastName;
                employee.DateofBirth = evm.DateofBirth;
                employee.Gender = evm.Gender;
                employee.Email = evm.Email;
                employee.Address = evm.Address;
                employee.Phone = evm.Phone;
                employee.PicturePath = filePath;

                _db.Entry(employee).State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(evm);
        }

        // GET: Employee/Delete/5
        public IActionResult Delete(int id)
        {
            var employee = _db.admin_Employee.Find(id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = _db.admin_Employee.Find(id);
            if (employee == null)
            {
                return NotFound();
            }

            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, employee.PicturePath);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _db.admin_Employee.Remove(employee);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
