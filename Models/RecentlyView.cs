﻿using System.ComponentModel.DataAnnotations;

namespace OnlineGasBooking.Models
{
    public class RecentlyView
    {
        [Key]
        public int RViewID { get; set; }
        public int CustomerID { get; set; }
        public int ProductID { get; set; }
        public System.DateTime ViewDate { get; set; }
        public string? Note { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Product Product { get; set; }
    }
}
