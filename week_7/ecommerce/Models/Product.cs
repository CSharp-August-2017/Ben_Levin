using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; //for using foreign key

namespace ecommerce.Models
{
    public class Product : BaseEntity
    {
        [Key]
        public int ID { get; set; }
        
        [MinLength(2, ErrorMessage="name must be at least two characters long")]
        [Required(ErrorMessage="product must have a name")]
        public string Name { get; set; }

        [DataType(DataType.ImageUrl)]
        [Required(ErrorMessage="product must have an image url")]
        [Display(Name="Image URL")]
        public string ImageURL { get; set; }

        [MinLength(10, ErrorMessage="description must be at least ten characters long")]
        [Required(ErrorMessage="product must have a description")]
        public string Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage="product may not have a negative quantity")]
        [Required(ErrorMessage="product must have a quantity")]
        public double Quantity { get; set; }

        [ForeignKey("User")]
        public int userID { get; set; }

        public DateTime? Created_At { get; set; }
        
        public DateTime? Updated_At { get; set; }
    }
}