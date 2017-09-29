using System;
using System.ComponentModel.DataAnnotations;

namespace user_dash.Models
{
    public class User : BaseEntity
    {
        [Key]
        public int ID { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public string Email { get; set; }
        
        public string Password { get; set; }

        public int AdminLevel { get; set; }

        public string Description { get; set; }
        
        public DateTime? Created_At { get; set; }        
        
        public DateTime? Updated_At { get; set; }
    }
}