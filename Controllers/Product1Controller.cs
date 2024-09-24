using OnlineGasBooking.Models;
//using OnlineGasBooking.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;
using X.PagedList.Extensions;

namespace OnlineGasBooking.Controllers
{
    
    
    
    
    public class Product1Controller : Controller
    {
        private readonly OnlineGasDBContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;

        // Constructor to initialize the database context and HttpContextAccessor
        public Product1Controller(OnlineGasDBContext db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            ViewBag.Categories = _db.Categories.Select(x => x.Name).ToList();
            ViewBag.SubCategories = _db.SubCategories.Select(x => x.Name).ToList();
            ViewBag.RecentViewsProducts = GetRecentViewProducts();

            return View("Products");
        }

        // RECENT VIEW PRODUCTS
        private IEnumerable<Product> GetRecentViewProducts()
        {
            if (TempShpData.UserID > 0)
            {
                var top3ProductIds = _db.RecentlyViews
                    .Where(recent => recent.CustomerID == TempShpData.UserID)
                    .OrderByDescending(recent => recent.ViewDate)
                    .Take(3)
                    .Select(recent => recent.ProductID)
                    .ToList();

                return _db.Products.Where(x => top3ProductIds.Contains(x.ProductID)).ToList();
            }
            else
            {
                return _db.Products
                    .OrderByDescending(x => x.UnitPrice)
                    .Take(3)
                    .ToList();
            }
        }

        // ADD TO CART
        public IActionResult AddToCart(int id)
        {
            var returnUrl="";
            if (HttpContext.Session.GetString("username")==null)
            {
                TempData["returnURL"] = "/Account/Login/";
                returnUrl = TempData["returnURL"]?.ToString() ?? "/";
                return Redirect(returnUrl);
            }
            var product = _db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            var orderDetail = new OrderDetail
            {
                ProductID = id,
                Quantity = 1,
                UnitPrice = product.UnitPrice,
                TotalAmount = product.UnitPrice,
                Product = product
            };

            if (TempShpData.items == null)
            {
                TempShpData.items = new List<OrderDetail>();
            }

            TempShpData.items.Add(orderDetail);
            AddRecentViewProduct(id);
            TempData["returnURL"] = "/Product1/ViewDetails/" + id;
            returnUrl = TempData["returnURL"]?.ToString() ?? "/";
            return Redirect(returnUrl);
        }

        // VIEW DETAILS
        public IActionResult ViewDetails(int id)
        {
            var product = _db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            var reviews = _db.Reviews.Where(x => x.ProductID == id).ToList();
            ViewBag.Reviews = reviews;
            ViewBag.TotalReviews = reviews.Count;
            ViewBag.RelatedProducts = _db.Products.Where(y => y.CategoryID == product.CategoryID).ToList();
            AddRecentViewProduct(id);

            var totalRate = reviews.Sum(x => x.Rate);
            ViewBag.AvgRate = reviews.Count > 0 ? totalRate / reviews.Count : 0;
            // Get data using the service
            //var data = _loadDataService.GetDefaultData();

            // Pass data to the view
            if (TempShpData.items == null)
            {
                TempShpData.items = new List<OrderDetail>();
            }
            int ncount= TempShpData.items.Count();
            ViewBag.NoOfItem = ncount; 
            ViewBag.Subtotal = HttpContext.Items["subtotal"];
            ViewBag.TotalAmount = HttpContext.Items["totalamount"];
            ViewBag.WishlistItemsNo = HttpContext.Items["wlitemsno"];

            return View(product);
        }

        // WISHLIST
        //public IActionResult WishList(int id)
        //{
        //    var wishlist = new Wishlist
        //    {
        //        ProductID = id,
        //        CustomerID = TempShpData.UserID
        //    };

        //    _db.Wishlists.Add(wishlist);
        //    _db.SaveChanges();
        //    AddRecentViewProduct(id);

        //    ViewBag.WlItemsNo = _db.Wishlists.Count(x => x.CustomerID == TempShpData.UserID);
        //    var returnUrl = TempData["returnURL"]?.ToString() ?? "/";
        //    return Redirect(returnUrl);
        //}

        // ADD RECENT VIEWS PRODUCT IN DB
        private void AddRecentViewProduct(int productId)
        {
            if (TempShpData.UserID > 0)
            {
                var recentView = new RecentlyView
                {
                    CustomerID = TempShpData.UserID,
                    ProductID = productId,
                    ViewDate = DateTime.Now
                };

                _db.RecentlyViews.Add(recentView);
                _db.SaveChanges();
            }
        }

        // ADD REVIEWS ABOUT PRODUCT
        [HttpPost]
        public IActionResult AddReview(int productID, [FromForm] Review review)
        {
            review.CustomerID = TempShpData.UserID;
            review.ProductID = productID;
            review.DateTime = DateTime.Now;

            _db.Reviews.Add(review);
            _db.SaveChanges();

            return RedirectToAction("ViewDetails", new { id = productID });
        }

        public IActionResult Products(int subCatID)
        {
            ViewBag.Categories = _db.Categories.Select(x => x.Name).ToList();
            var products = _db.Products.Where(y => y.SubCategoryID == subCatID).ToList();
            return View(products);
        }

        // GET PRODUCTS BY CATEGORY
        public IActionResult GetProductsByCategory(string categoryName, int? page)
        {
            ViewBag.Categories = _db.Categories.Select(x => x.Name).ToList();
            ViewBag.SubCategories = _db.SubCategories.Select(x => x.Name).ToList();
            ViewBag.RecentViewsProducts = GetRecentViewProducts();

            var products = _db.Products
                .Where(x => x.SubCategory.Name == categoryName)
                .ToPagedList(page ?? 1, 9);

            return View("Products", products);
        }

        // SEARCH BAR RESULT
        public IActionResult Search(string product, int? page)
        {
            ViewBag.SubCategories = _db.SubCategories.Select(x => x.Name).ToList();
            ViewBag.RecentViewsProducts = GetRecentViewProducts();

            var products = !string.IsNullOrEmpty(product)
                ? _db.Products.Where(x => x.Name.StartsWith(product)).ToList()
                : _db.Products.ToList();

            return View("Products", products.ToPagedList(page ?? 1, 6));
        }

        public JsonResult GetProducts(string term)
        {
            var productNames = _db.Products
                .Where(x => x.Name.StartsWith(term))
                .Select(y => y.Name)
                .ToList();

            return Json(productNames);
        }

        // FILTER PRODUCTS BY PRICE
        public IActionResult FilterByPrice(int minPrice, int maxPrice, int? page)
        {
            ViewBag.SubCategories = _db.SubCategories.Select(x => x.Name).ToList();
            ViewBag.RecentViewsProducts = GetRecentViewProducts();
            ViewBag.FilterByPrice = true;

            var filteredProducts = _db.Products
                .Where(x => x.UnitPrice >= minPrice && x.UnitPrice <= maxPrice)
                .ToPagedList(page ?? 1, 9);

            return View("Products", filteredProducts);
        }
    }
}
