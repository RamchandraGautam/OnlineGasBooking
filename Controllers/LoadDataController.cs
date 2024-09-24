using OnlineGasBooking.Models;
using Microsoft.AspNetCore.Mvc;

namespace OnlineGasBooking.Controllers
{
    
    
    
    
    public static class LoadDataController
    {

        //private readonly  OnlineGasDBContext _dbContext;
        //private readonly IHttpContextAccessor _httpContextAccessor;

        //public LoadDataService(OnlineGasDBContext dbContext, IHttpContextAccessor httpContextAccessor)
        //{
        //    _dbContext = dbContext;
        //    _httpContextAccessor = httpContextAccessor;
        //}

        public static List<OrderDetail> GetDefaultData(this ControllerBase controller)
        {
            if (TempShpData.items == null)
            {
                TempShpData.items = new List<OrderDetail>();
            }
            var data = TempShpData.items.ToList();

           // ViewBag.cartBox = data.Count == 0 ? null : data;
           //ViewBag.NoOfItem = data.Count();
           // int? SubTotal = Convert.ToInt32(data.Sum(x => x.TotalAmount));
           // controller.ViewBag.Total = SubTotal;

           // int Discount = 0;
           // controller.ViewBag.SubTotal = SubTotal;
           // controller.ViewBag.Discount = Discount;
           // controller.ViewBag.TotalAmount = SubTotal - Discount;

           // controller.ViewBag.WlItemsNo = _db.Wishlists.Where(x => x.CustomerID == TempShpData.UserID).ToList().Count();

            return data;
        }
    }
}
