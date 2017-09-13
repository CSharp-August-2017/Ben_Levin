using System;
using System.ComponentModel.DataAnnotations;
 
namespace woods.Models
{
    public class Trail : BaseEntity
    
    {
        public int ID { get; set; }

        [Required(ErrorMessage="Trail Name is required")]
        [Display(Name = "Trail Name")]
        public string Name { get; set; }

        [MinLength(10, ErrorMessage="Description must contain at least 10 characters")]
        [Display(Name = "Description")] //not necessary
        public string Description { get; set; }

        [RegularExpression(@"^[0-9]+$", ErrorMessage="Trail Length must be a number")]
        [Display(Name = "Trail Length")]
        public string Length { get; set; }

        [RegularExpression(@"^[0-9]+$", ErrorMessage="Elevation Change must be a number")]
        [Display(Name = "Elevation Change")]
        public string Elevation { get; set; }

        [RegularExpression(@"^(\+|-)?(?:180(?:(?:\.0{1,6})?)|(?:[0-9]|[1-9][0-9]|1[0-7][0-9])(?:(?:\.[0-9]{1,6})?))$", ErrorMessage="Longitude must be in proper format")]
        [Display(Name = "Longitude")] //not necessary
        public string Longitude { get; set; }

        [RegularExpression(@"^(\+|-)?(?:90(?:(?:\.0{1,6})?)|(?:[0-9]|[1-8][0-9])(?:(?:\.[0-9]{1,6})?))$", ErrorMessage="Latitude must be in proper format")]
        [Display(Name = "Latitude")] //not necessary
        public string Latitude { get; set; }
    }
}