using System;
using System.ComponentModel.DataAnnotations;
 
namespace dojo_league.Models
{
    public class Ninja : BaseEntity
    
    {   
        public long ID { get; set; }
        
        [Required(ErrorMessage="A name is required")]
        [MinLength(2, ErrorMessage="Name must be at least two characters long")]
        [Display(Name="Ninja Name")]
        public string Name { get; set; }

        [Required(ErrorMessage="A ninjaing level is required")]
        [Range(1,10, ErrorMessage="Level must be between 1 and 10")]
        [Display(Name="Ninjaing Level")]
        public string Level { get; set; }

        [Display(Name="Ninja Description")]
        public string Description { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }

        public string DojoID { get; set; } //having issues creating records using dojo field, need to revisit

        public Dojo dojo { get; set; }
    }
}