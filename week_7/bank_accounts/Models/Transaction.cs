using System;
using System.ComponentModel.DataAnnotations; //handling validations in controller
namespace bank_accounts.Models
{
    public class Transaction : BaseEntity
    {
        public int ID { get; set; }

        [DataType(DataType.Currency)]
        public double Amount { get; set; }

        public int? UserID { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}