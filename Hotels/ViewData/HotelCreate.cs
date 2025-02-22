using System;
using System.ComponentModel.DataAnnotations;

namespace Hotels.ViewData
{
    public class HotelCreate
    {
        [Required]
        [StringLength(20, ErrorMessage = "Name must be less than 20 characters.")]
        public string Name { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Price must have up to 2 decimal places.")]
        public double Price { get; set; }

        [Required]
        [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90.")]
        [RegularExpression(@"^-?\d{1,2}(\.\d{1,2})?$", ErrorMessage = "Latitude must have up to 2 decimal places.")]
        public double Latitude { get; set; }

        [Required]
        [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180.")]
        [RegularExpression(@"^-?\d{1,3}(\.\d{1,2})?$", ErrorMessage = "Longitude must have up to 2 decimal places.")]
        public double Longitude { get; set; }
    }
}
