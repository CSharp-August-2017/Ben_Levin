using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace wedding_planner.Models
{
    public class Wedding : BaseEntity
    {
        public Wedding() 
        {
            Guests = new List<User>();
        }
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage="name of wedder one is required")]
        [Display(Name="Wedder One")]
        public string WedderOne { get; set; }
        [Required(ErrorMessage="name of wedder two is required")]
        [Display(Name="Wedder Two")]
        public string WedderTwo { get; set; }

        [Required(ErrorMessage="wedding date is required")]
        [Display(Name="Wedding Date")]
        [DataType(DataType.Date)]
        public DateTime? WeddingDate { get; set; }

        [Required(ErrorMessage="wedding address is required")]
        [Display(Name="Wedding Address")]
        public string WeddingAddress { get; set; }

        public int UserID { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ICollection<User> Guests { get; set; }
    }
}