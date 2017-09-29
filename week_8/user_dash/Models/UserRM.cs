using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; //for using foreign key


namespace user_dash.Models
{
    public class UserRenderModel : BaseEntity
    {
        public int ID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Description { get; set; }

        public string CreatedDate { get; set; }

        public string AdminLevel { get; set; }
    }
}