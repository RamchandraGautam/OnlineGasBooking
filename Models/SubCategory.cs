﻿using System.ComponentModel.DataAnnotations;

namespace OnlineGasBooking.Models
{
    public partial class SubCategory
    {
        public SubCategory()
        {
            this.Products = new HashSet<Product>();
        }
        [Key]
        public int SubCategoryID { get; set; }
        public int CategoryID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Nullable<bool> isActive { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
