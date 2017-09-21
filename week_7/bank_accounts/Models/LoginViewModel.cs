using System.ComponentModel.DataAnnotations;
namespace bank_accounts.Models
{
    public class LoginViewModel : BaseEntity //likely better to handle validations in controller
    {
        [Required(ErrorMessage="invalid e-mail address")]
        [EmailAddress(ErrorMessage="invalid e-mail address")]
        [Display(Name="E-mail Address")]
        public string Email { get; set; }
 
        [Required(ErrorMessage="invalid password")]
        [MinLength(8, ErrorMessage="invalid password")]
        [DataType(DataType.Password, ErrorMessage="invalid password")]
        [Display(Name="Password")]
        public string Password { get; set; }
    }
}