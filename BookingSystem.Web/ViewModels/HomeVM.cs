using BookingSystem.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace BookingSystem.Web.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Villa> ? VillaList { get; set; }
        
        [Required(ErrorMessage = "Check-in date is required")]
        [Display(Name = "Check-in Date")]
        public DateOnly CheckInDate { get; set; }
        
        public DateOnly CheckOutDate { get; set; }
        
        [Required(ErrorMessage = "Number of nights is required")]
        [Range(1, 10, ErrorMessage = "Number of nights must be between 1 and 10")]
        [Display(Name = "Number of Nights")]
        public int Nights { get; set; }

    }
}
