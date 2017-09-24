using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; //for using foreign key

namespace ecommerce.Models
{
    public class Customer : BaseEntity
    {
        [Key]
        public int ID { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage="first name may only contain letters")]
        [MinLength(2, ErrorMessage="first name must be at least two characters long")]
        [Required(ErrorMessage="customer must have a first name")]
        [Display(Name="First Name")]
        public string FirstName { get; set; }
        
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage="last name may only contain letters")]
        [MinLength(2, ErrorMessage="last name must be at least two characters long")]
        [Required(ErrorMessage="customer must have a last name")]
        [Display(Name="Last Name")]
        public string LastName { get; set; }

        [ForeignKey("User")]
        public int userID { get; set; }

        public int Status { get; set; }

        public DateTime? Created_At { get; set; }

        public DateTime? Updated_At { get; set; }
    }
}