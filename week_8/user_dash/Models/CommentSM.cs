using System;
using System.ComponentModel.DataAnnotations;


namespace user_dash.Models
{
    public class CommentSendModel : BaseEntity
    {
        [Key]
        public int ID { get; set; }
        
        [Display(Name="write a comment")]
        [MinLength(2, ErrorMessage="comment must contain at least two characters")]
        [Required(ErrorMessage="comment may not be blank")]
        public string CContent { get; set; }
        
        public int AuthorID { get; set; }

        public int UserID { get; set; }
     
        public int MessageID { get; set; }
        
        public DateTime? Created_At { get; set; }        
        
        public DateTime? Updated_At { get; set; }
    }
}