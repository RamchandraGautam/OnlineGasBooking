﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineGasBooking.Models
{
   
    public class ProductVM
    {
        [Key]
        public int ProductID { get; set; }
        [Required, Display(Name = "Product Name")]
        public string Name { get; set; }
        [Required, Display(Name = "Supplier")]
        public int SupplierID { get; set; }
        [Required, Display(Name = "Category")]
        public int CategoryID { get; set; }
        [Display(Name = "SubCategory")]
        public Nullable<int> SubCategoryID { get; set; }
        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }
        [Required, Display(Name = "Previous Price")]
        public Nullable<decimal> OldPrice { get; set; }
        public Nullable<decimal> Discount { get; set; }
        [Display(Name = "Stock")]
        public Nullable<int> UnitInStock { get; set; }
        [Display(Name = "Available?")]
        public Nullable<bool> ProductAvailable { get; set; }
        [Display(Name = "Description")]
        public string ShortDescription { get; set; }
        [Display(Name = "Picture")]
        public string? PicturePath { get; set; }

        [NotMapped]
        public IFormFile? Picture { get; set; }
        public string? Note { get; set; }
    }
}
