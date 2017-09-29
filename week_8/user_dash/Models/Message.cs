using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; //for using foreign key


namespace user_dash.Models
{
    public class Message : BaseEntity
    {
        [Key]
        public int ID { get; set; }
        
        [ForeignKey("User")]
        public int AuthorID { get; set; }

        [ForeignKey("User")]        
        public int UserID { get; set; }

        [MinLength(2, ErrorMessage="message must contain at least two characters")]
        [Display(Name="write a message")]
        [Required(ErrorMessage="message may not be blank")]
        public string Content { get; set; }
        
        public DateTime? Created_At { get; set; }        
        
        public DateTime? Updated_At { get; set; }
    }
}