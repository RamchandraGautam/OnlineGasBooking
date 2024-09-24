using OnlineGasBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineGasBooking.Controllers
{
    
    
    
    
    public class ProductController : Controller
    {
        private readonly OnlineGasDBContext _db;
        private readonly IWebHostEnvironment _hostingEnvironment;

        // Constructor to initialize the database context and hosting environment
        public ProductController(OnlineGasDBContext db, IWebHostEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Product
        public IActionResult Index()
        {
            return View(_db.Products.ToList());
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            ViewBag.SupplierList = new SelectList(_db.Suppliers, "SupplierID", "CompanyName");
            ViewBag.CategoryList = new SelectList(_db.Categories, "CategoryID", "Name");
            ViewBag.SubCategoryList = new SelectList(_db.SubCategories, "SubCategoryID", "Name");
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductVM pvm)
        {
            if (ModelState.IsValid)
            {
                string filePath = null;
                if (pvm.Picture != null)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(pvm.Picture.FileName);
                    var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "Images");
                    filePath = Path.Combine("Images", fileName);
                    var filePathFull = Path.Combine(uploads, fileName);

                    using (var stream = new FileStream(filePathFull, FileMode.Create))
                    {
                        await pvm.Picture.CopyToAsync(stream);
                    }
                }

                var product = new Product
                {
                    ProductID = pvm.ProductID,
                    Name = pvm.Name,
                    SupplierID = pvm.SupplierID,
                    CategoryID = pvm.CategoryID,
                    SubCategoryID = pvm.SubCategoryID,
                    UnitPrice = pvm.UnitPrice,
                    OldPrice = pvm.OldPrice,
                    Discount = pvm.Discount,
                    UnitInStock = pvm.UnitInStock,
                    ProductAvailable = pvm.ProductAvailable,
                    ShortDescription = pvm.ShortDescription,
                    Note = pvm.Note,
                    PicturePath = filePath
                };

                _db.Products.Add(product);
                await _db.SaveChangesAsync();

                return PartialView("_Success");
            }

            ViewBag.SupplierList = new SelectList(_db.Suppliers, "SupplierID", "CompanyName");
            ViewBag.CategoryList = new SelectList(_db.Categories, "CategoryID", "Name");
            ViewBag.SubCategoryList = new SelectList(_db.SubCategories, "SubCategoryID", "Name");
            return PartialView("_Error");
        }

        // GET: Product/Edit/5
        public IActionResult Edit(int id)
        {
            var product = _db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            var pvm = new ProductVM
            {
                ProductID = product.ProductID,
                Name = product.Name,
                SupplierID = product.SupplierID,
                CategoryID = product.CategoryID,
                SubCategoryID = product.SubCategoryID,
                UnitPrice = product.UnitPrice,
                OldPrice = product.OldPrice,
                Discount = product.Discount,
                UnitInStock = product.UnitInStock,
                ProductAvailable = product.ProductAvailable,
                ShortDescription = product.ShortDescription,
                Note = product.Note,
                PicturePath = product.PicturePath
            };

            ViewBag.SupplierList = new SelectList(_db.Suppliers, "SupplierID", "CompanyName");
            ViewBag.CategoryList = new SelectList(_db.Categories, "CategoryID", "Name");
            ViewBag.SubCategoryList = new SelectList(_db.SubCategories, "SubCategoryID", "Name");
            return View(pvm);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductVM pvm)
        {
            if (ModelState.IsValid)
            {
                var product = _db.Products.Find(pvm.ProductID);
                if (product == null)
                {
                    return NotFound();
                }

                string filePath = pvm.PicturePath;
                if (pvm.Picture != null)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(pvm.Picture.FileName);
                    var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "Images");
                    filePath = Path.Combine("Images", fileName);
                    var filePathFull = Path.Combine(uploads, fileName);

                    using (var stream = new FileStream(filePathFull, FileMode.Create))
                    {
                        await pvm.Picture.CopyToAsync(stream);
                    }
                }

                product.Name = pvm.Name;
                product.SupplierID = pvm.SupplierID;
                product.CategoryID = pvm.CategoryID;
                product.SubCategoryID = pvm.SubCategoryID;
                product.UnitPrice = pvm.UnitPrice;
                product.OldPrice = pvm.OldPrice;
                product.Discount = pvm.Discount;
                product.UnitInStock = pvm.UnitInStock;
                product.ProductAvailable = pvm.ProductAvailable;
                product.ShortDescription = pvm.ShortDescription;
                product.Note = pvm.Note;
                product.PicturePath = filePath;

                _db.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.SupplierList = new SelectList(_db.Suppliers, "SupplierID", "CompanyName");
            ViewBag.CategoryList = new SelectList(_db.Categories, "CategoryID", "Name");
            ViewBag.SubCategoryList = new SelectList(_db.SubCategories, "SubCategoryID", "Name");
            return View(pvm);
        }

        // GET: Product/Details/5
        public IActionResult Details(int id)
        {
            var product = _db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            var pvm = new ProductVM
            {
                ProductID = product.ProductID,
                Name = product.Name,
                SupplierID = product.SupplierID,
                CategoryID = product.CategoryID,
                SubCategoryID = product.SubCategoryID,
                UnitPrice = product.UnitPrice,
                OldPrice = product.OldPrice,
                Discount = product.Discount,
                UnitInStock = product.UnitInStock,
                ProductAvailable = product.ProductAvailable,
                ShortDescription = product.ShortDescription,
                Note = product.Note,
                PicturePath = product.PicturePath
            };

            ViewBag.SupplierList = new SelectList(_db.Suppliers, "SupplierID", "CompanyName");
            ViewBag.CategoryList = new SelectList(_db.Categories, "CategoryID", "Name");
            ViewBag.SubCategoryList = new SelectList(_db.SubCategories, "SubCategoryID", "Name");
            return View(pvm);
        }

        // POST: Product/Details/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(ProductVM pvm)
        {
            if (ModelState.IsValid)
            {
                var product = _db.Products.Find(pvm.ProductID);
                if (product == null)
                {
                    return NotFound();
                }

                string filePath = pvm.PicturePath;
                if (pvm.Picture != null)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(pvm.Picture.FileName);
                    var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "Images");
                    filePath = Path.Combine("Images", fileName);
                    var filePathFull = Path.Combine(uploads, fileName);

                    using (var stream = new FileStream(filePathFull, FileMode.Create))
                    {
                        await pvm.Picture.CopyToAsync(stream);
                    }
                }

                product.Name = pvm.Name;
                product.SupplierID = pvm.SupplierID;
                product.CategoryID = pvm.CategoryID;
                product.SubCategoryID = pvm.SubCategoryID;
                product.UnitPrice = pvm.UnitPrice;
                product.OldPrice = pvm.OldPrice;
                product.Discount = pvm.Discount;
                product.UnitInStock = pvm.UnitInStock;
                product.ProductAvailable = pvm.ProductAvailable;
                product.ShortDescription = pvm.ShortDescription;
                product.Note = pvm.Note;
                product.PicturePath = filePath;

                _db.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.SupplierList = new SelectList(_db.Suppliers, "SupplierID", "CompanyName");
            ViewBag.CategoryList = new SelectList(_db.Categories, "CategoryID", "Name");
            ViewBag.SubCategoryList = new SelectList(_db.SubCategories, "SubCategoryID", "Name");
            return View(pvm);
        }

        // GET: Product/Delete/5
        public IActionResult Delete(int id)
        {
            var product = _db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = _db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, product.PicturePath);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
