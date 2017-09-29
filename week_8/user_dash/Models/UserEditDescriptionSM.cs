using System.ComponentModel.DataAnnotations;

namespace user_dash.Models
{
    public class UserEditDescriptionSendModel : BaseEntity
    {
        public int ID { get; set; }

        [Display(Name="Edit Description")]
        [MinLength(10, ErrorMessage="description must be at least ten characters long")]
        public string EdDescription { get; set; }
    }
}