using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
 
namespace dojo_league.Models
{
    public class Dojo : BaseEntity
    
    {
        public Dojo()
        {
            ninjas = new List<Ninja>();
        } 

        [Key]
        public long ID { get; set; }
        
        [Required(ErrorMessage="A name is required")]
        [MinLength(2, ErrorMessage="Name must be at least two characters long")]
        [Display(Name="Dojo Name")]
        public string Name { get; set; }

        [Required(ErrorMessage="A location is required")]
        [Display(Name="Dojo Location")]
        public string Location { get; set; }

        [Display(Name="Additional Dojo Information")]
        public string Info { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }

        public ICollection<Ninja> ninjas { get; set; }
    }
}