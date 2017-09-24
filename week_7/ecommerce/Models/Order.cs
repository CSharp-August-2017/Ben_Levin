using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; //for using foreign key

namespace ecommerce.Models
{
    public class Order : BaseEntity
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("User")]
        public int userID { get; set; }

        [ForeignKey("Customer")]
        public int customerID { get; set; }

        [ForeignKey("Product")]
        public int productID { get; set; }
        
        [Range(0, double.MaxValue)]
        public double ProductQuantity { get; set; }
        
        public DateTime? Created_At { get; set; }
        
        public DateTime? Updated_At { get; set; }
    }
}