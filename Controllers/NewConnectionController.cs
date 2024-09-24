using OnlineGasBooking.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace OnlineGasBooking.Controllers
{
    public class NewConnectionController : Controller
    {
        private readonly OnlineGasDBContext _db;

        // Constructor to initialize the database context
        public NewConnectionController(OnlineGasDBContext db) => _db = db;

        public IActionResult Index()
        {
            //var UserID= HttpContext.Session.GetInt32("CustomerID");
            //var ConnectionList = _db.NewConnection.ToList();
            //_db.Products.ToList()
            return View(_db.NewConnection.ToList());
        }

        public IActionResult NewConnectionView()
        {
            HttpContext.Session.SetString("username", "");
            var usr = _db.Customers.Find(TempShpData.UserID);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NewConnectionView(NewConnectionVM NewconData)
        {

            try
            {
                if (NewconData.Picture != null)
                {
                    // Handle file upload
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", Guid.NewGuid() + Path.GetExtension(NewconData.Picture.FileName));

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        NewconData.Picture.CopyTo(stream);
                    }

                    var Connection = new NewConnection
                    {
                        NewConnectionID = NewconData.NewConnectionID,
                        First_Name = NewconData.First_Name,
                        Last_Name = NewconData.Last_Name,
                        Gender = NewconData.Gender,
                        DateofBirth = NewconData.DateofBirth,
                        Country = NewconData.Country,
                        City = NewconData.City,
                        PostalCode = NewconData.PostalCode,
                        Email = NewconData.Email,
                        Phone = NewconData.Phone,
                        Address = NewconData.Address,
                        DocPicturePath = "/Images/" + Path.GetFileName(filePath),
                        status = "Pending",
                        CustomerID = HttpContext.Session.GetInt32("CustomerID")
                    };

                    _db.NewConnection.Add(Connection);
                    _db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                // Log the exception and show error view
                // _logger.LogError(ex, "Error while registering customer.");
                return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
            }
            return View(NewconData);
        }
        public IActionResult Details(int id)
        {
            var product = _db.NewConnection.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        public IActionResult Delete(int id)
        {
            var product = _db.NewConnection.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
            //if (TempShpData.Connections != null)
            //{
            //    TempShpData.Connections.RemoveAll(x => x.NewConnectionID == id);
            //}
            //return RedirectToAction("Index");
        }

        // POST: Connection/Delete/
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = _db.NewConnection.Find(id);
            if (product == null)
            {
                return NotFound();
            }           
            _db.NewConnection.Remove(product);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult ProcedToCheckout(Dictionary<string, string> formData)
        {
            if (TempShpData.items == null)
            {
                TempShpData.items = new List<OrderDetail>();
            }

            var updatedItems = new List<OrderDetail>();

            foreach (var key in formData.Keys)
            {
                if (key.StartsWith("shcartID-"))
                {
                    int index = int.Parse(key.Substring("shcartID-".Length));
                    int productId = int.Parse(formData[key]);

                    var orderDetail = TempShpData.items.FirstOrDefault(x => x.ProductID == productId);

                    if (orderDetail != null)
                    {
                        int quantity = int.Parse(formData[$"Qty-{index}"]);
                        orderDetail.Quantity = quantity;
                        orderDetail.TotalAmount = quantity * orderDetail.UnitPrice;

                        // Remove old entry and add updated entry
                        TempShpData.items.RemoveAll(x => x.ProductID == productId);
                        updatedItems.Add(orderDetail);
                    }
                }
            }

            // Add updated items to TempShpData
            TempShpData.items.AddRange(updatedItems);

            return RedirectToAction("Index", "CheckOut");
        }

        private IEnumerable<OrderDetail> GetDefaultData()
        {
            // Implementation for fetching default data. This is just a placeholder.
            // Return appropriate data for your cart.
            return TempShpData.items ?? new List<OrderDetail>();
        }
    }
}
