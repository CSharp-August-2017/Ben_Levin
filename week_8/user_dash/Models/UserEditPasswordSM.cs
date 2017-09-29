using System.ComponentModel.DataAnnotations;

namespace user_dash.Models
{
    public class UserEditPasswordSendModel : BaseEntity
    {
        public int ID { get; set; }
        
        public string EdEmail { get; set; }
 
        [Required(ErrorMessage="password is required")]
        [MinLength(8, ErrorMessage="password must be at least eight characters long")]
        [DataType(DataType.Password)]
        [Display(Name="Password")]     
        public string EdPassword { get; set; }
 
        [Compare("EdPassword", ErrorMessage = "passwords must match")]
        [DataType(DataType.Password)]
        [Display(Name="Confirm Password")]        
        public string EdPasswordConfirmation { get; set; }
    }
}