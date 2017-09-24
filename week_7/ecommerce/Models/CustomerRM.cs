namespace ecommerce.Models
{
    public class CustomerRenderModel : BaseEntity
    {
        public int ID { get; set; }

        public string CompleteName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        
        public string Date { get; set; }

        public string HoursAgo { get; set; }

        public int Status { get; set; }
    }
}