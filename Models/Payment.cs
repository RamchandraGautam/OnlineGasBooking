using System.ComponentModel.DataAnnotations;

namespace OnlineGasBooking.Models
{
    public partial class Payment
    {
        
        [Key]
        public int PaymentID { get; set; }
        public string? PaymentStatus { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<System.DateTime> PaymentDateTime { get; set; }       
        public virtual PaymentType? PaymentType { get; set; }
    }
}
