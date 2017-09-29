using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; //for using foreign key


namespace user_dash.Models
{
    public class Comment : BaseEntity
    {
        [Key]
        public int ID { get; set; }
        
        public string Content { get; set; }
        
        [ForeignKey("User")]
        public int AuthorID { get; set; }

        [ForeignKey("Message")]        
        public int MessageID { get; set; }
        
        public DateTime? Created_At { get; set; }        
        
        public DateTime? Updated_At { get; set; }
    }
}