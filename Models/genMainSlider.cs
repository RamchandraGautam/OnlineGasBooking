using System.ComponentModel.DataAnnotations;

namespace OnlineGasBooking.Models
{
    public partial class genMainSlider
    {
        [Key]
        public int MainSliderID { get; set; }

        public string? ImageURL { get; set; } // Make ImageURL nullable
        public string? AltText { get; set; } // Make AltText nullable
        public string? OfferTag { get; set; } // Make OfferTag nullable
        public string? Title { get; set; } // Make Title nullable
        public string? Description { get; set; } // Make Description nullable
        public string? BtnText { get; set; } // Make BtnText nullable
        public bool? isDeleted { get; set; } // Use bool? for nullable boolean
    }
}
