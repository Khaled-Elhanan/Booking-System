using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.Domain.Entities
{
    public class Villa
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string ? Description { get; set; }
        [Display(Name="Price per night")]
        public double Price { get; set; }

        public int Sqft { get; set; }
        public int Occupancy { get; set; }
        [NotMapped]
        public IFormFile ? Image { get; set; }
        public string ? ImageUrl { get; set; }

        public DateTime CreatedDate { get; set; } 
        public DateTime UpdatedDate { get; set; }

        [ValidateNever]
        public  IEnumerable<Amenity> VillaAmenities { get; set; }

        [ValidateNever]
        public IEnumerable<VillaNumber> VillaNumbers { get; set; }

        [NotMapped]
        public bool IsAvailable { get; set; } = true;
    }
}
