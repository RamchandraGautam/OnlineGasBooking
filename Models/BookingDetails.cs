namespace OnlineGasBooking.Models
{
    public class BookingDetails
    {
        public int OrderID { get; set; }
        public int PaymentID { get; set; }
        
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string? PaymentStatus { get; set; }
        public bool? isCompleted { get; set; }
        public string? Address { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }       
        public Nullable<System.DateTime> ShippingDate { get; set; }       
       
        public string? Notes { get; set; }
        
    }
}
