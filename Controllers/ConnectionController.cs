using OnlineGasBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineGasBooking.Controllers
{
    public class ConnectionController : Controller
    {
        private readonly OnlineGasDBContext _db;

        // Constructor to initialize the database context
        public ConnectionController(OnlineGasDBContext db) => _db = db;

        // GET: Supplier
        public async Task<IActionResult> Index()
        {
            //var suppliers = await _db.NewConnection.ToListAsync();
            ViewBag.latestOrders = await _db.NewConnection.ToListAsync();// _db.NewConnection.OrderByDescending(x => x.NewConnectionID).Take(10).ToList();

            return View();
        }

        // CREATE: Supplier
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                _db.Suppliers.Add(supplier);
                await _db.SaveChangesAsync();
                return PartialView("_Success");
            }
            return PartialView("_Error");
        }

        // EDIT: Supplier
        public async Task<IActionResult> Edit(int id)
        {
            var supplier = await _db.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(supplier).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(supplier);
        }

        // DETAILS: Supplier
        public async Task<IActionResult> Details(int id)
        {
            var supplier = await _db.NewConnection.FindAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }
        [HttpPost]
        public async Task<IActionResult> Action(NewConnection NCon)
        {
            
            try
            {
                await UpdateConnectionAsync(NCon.NewConnectionID, NCon.status);
                return PartialView("_Success");
            }
            catch (Exception ex)
            {

                return PartialView("_Error");
            }
        }
        public async Task UpdateConnectionAsync(int id, string newAction)
        {
            // Retrieve the entity
            var connection = await _db.NewConnection.FindAsync(id);

            if (connection != null)
            {
                // Update the property
                connection.status = newAction;

                // Save the changes to the database
                await _db.SaveChangesAsync();
            }
        }

        // DELETE: Supplier
        public async Task<IActionResult> Delete(int id)
        {
            var supplier = await _db.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var supplier = await _db.Suppliers.FindAsync(id);
            if (supplier != null)
            {
                _db.Suppliers.Remove(supplier);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
