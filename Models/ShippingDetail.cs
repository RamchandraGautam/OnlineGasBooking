﻿using System.ComponentModel.DataAnnotations;

namespace OnlineGasBooking.Models
{
    public partial class ShippingDetail
    {
        public ShippingDetail()
        {
            this.Orders = new HashSet<Order>();
        }
        [Key]
        public int ShippingID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? PostCode { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
