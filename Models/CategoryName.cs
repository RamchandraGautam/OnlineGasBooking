using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineGasBooking.Models
{
    [NotMapped]
    public class CategoryName
    {

        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
