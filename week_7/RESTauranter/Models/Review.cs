using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace RESTauranter.Models

{
    public class Review
    {
        public int ID { get; set; }
        
        [Required(ErrorMessage="a reviewer name is required")]
        [Display(Name="Reviewer Name")]
        public string Name { get; set; }
        
        [Required(ErrorMessage="a restaurant name is required")]
        [Display(Name="Restaurant Name")]
        public string Restaurant { get; set; }

        public int Stars { get; set; }
        
        [Required(ErrorMessage="a review is required")]
        [MinLength(10, ErrorMessage="review must be at least ten characters")]
        [Display(Name="Review")]
        public string ReviewText { get; set; }

        [Required(ErrorMessage="a date of visit is required")]
        [DataType(DataType.Date, ErrorMessage="date must be valid")]
        [Display(Name="Date of Visit")]

        public DateTime Visit { get; set; }

        public DateTime Created_At { get; set; }

        public DateTime Updated_At { get; set; }
    }
}