using System.ComponentModel.DataAnnotations;

namespace user_dash.Models
{
    public class UserEditInfoSendModel : BaseEntity
    {
        public int ID { get; set; }

        [Required(ErrorMessage="first name is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage="first name may only contain letters")]
        [MinLength(2, ErrorMessage="first name must be at least two characters long")]
        [Display(Name="First Name")]
        public string EdFirstName { get; set; }

        [Required(ErrorMessage="last name is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage="last name may only contain letters")]
        [MinLength(2, ErrorMessage="last name must be at least two characters long")]
        [Display(Name="Last Name")]
        public string EdLastName { get; set; }

        [Required(ErrorMessage="e-mail is required")]
        [EmailAddress(ErrorMessage="e-mail address must be a valid format")]
        [Display(Name="E-mail Address")]
        public string EdEmail { get; set; }

        [Display(Name="User Level")]
        public int EdUserLevel { get; set; }
    }
}