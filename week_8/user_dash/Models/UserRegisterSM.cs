using System.ComponentModel.DataAnnotations;

namespace user_dash.Models
{
    public class RegisterSendModel : BaseEntity
    {
        [Required(ErrorMessage="first name is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage="first name may only contain letters")]
        [MinLength(2, ErrorMessage="first name must be at least two characters long")]
        [Display(Name="First Name")]
        public string RegFirstName { get; set; }

        [Required(ErrorMessage="last name is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage="last name may only contain letters")]
        [MinLength(2, ErrorMessage="last name must be at least two characters long")]
        [Display(Name="Last Name")]
        public string RegLastName { get; set; }

        [Required(ErrorMessage="e-mail address is required")]
        [EmailAddress(ErrorMessage="e-mail address must be a valid format")]
        [Display(Name="E-mail Address")]
        public string RegEmail { get; set; }
 
        [Required(ErrorMessage="password is required")]
        [MinLength(8, ErrorMessage="password must be at least eight characters long")]
        [DataType(DataType.Password)]
        [Display(Name="Password")]     
        public string RegPassword { get; set; }
 
        [Compare("RegPassword", ErrorMessage = "passwords must match")]
        [DataType(DataType.Password)]
        [Display(Name="Confirm Password")]        
        public string RegPasswordConfirmation { get; set; }
    }
}