using System.ComponentModel.DataAnnotations;

namespace ecommerce.Models
{
    public class LoginViewModel : BaseEntity
    {
        [Required(ErrorMessage="invalid e-mail address")]
        [EmailAddress(ErrorMessage="invalid e-mail address")]
        [Display(Name="E-mail Address")]
        public string LogEmail { get; set; }
 
        [Required(ErrorMessage="invalid password")]
        [MinLength(8, ErrorMessage="invalid password")]
        [DataType(DataType.Password, ErrorMessage="invalid password")]
        [Display(Name="Password")]

        public string LogPassword { get; set; }
    }
}