using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineGasBooking.Models
{
    public class NewConnectionVM
    {
        [Key]
        public int NewConnectionID { get; set; }
        [Display(Name = "First Name")]
        public string? First_Name { get; set; }
        [Display(Name = "Last Name")]
        public string? Last_Name { get; set; }
        public string? Gender { get; set; }
        public Nullable<System.DateTime> DateofBirth { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        [Display(Name = "Postal Code")]
        public string? PostalCode { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        [Display(Name = "Picture")]
        //public string? DocPicturePath { get; set; }
        [NotMapped]
        public IFormFile? Picture { get; set; }
        //public string? status { get; set; }
    }
}
