using System.ComponentModel.DataAnnotations;

namespace wedding_planner.Models
{
    public class RegisterViewModel : BaseEntity
    {
        [Required(ErrorMessage="first name is required")]
        [MinLength(2, ErrorMessage="first name must be at least two characters long")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage="first name may only contain letters")]
        [Display(Name="First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage="last name is required")]
        [MinLength(2, ErrorMessage="last name must be at least two characters long")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage="last name may only contain letters")]
        [Display(Name="Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage="e-mail address is required")]
        [EmailAddress(ErrorMessage="e-mail address must be a valid format")]
        [Display(Name="E-mail Address")]
        public string Email { get; set; }
 
        [Required(ErrorMessage="password is required")]
        [MinLength(8, ErrorMessage="password must be at least eight characters long")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
 
        [Compare("Password", ErrorMessage = "passwords must match")]
        [DataType(DataType.Password)]
        [Display(Name="Password Confirmation")]
        public string PasswordConfirmation { get; set; }
    }
}