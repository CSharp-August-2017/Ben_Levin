namespace ecommerce.Models
{
    public class OrderRenderModel : BaseEntity
    {
        public string CustomerName { get; set; }

        public int CustomerStatus { get; set; }

        public string ProductName { get; set; }

        public int ProductQuantity { get; set; }

        public string Date { get; set; }

        public string HoursAgo { get; set; }
    }
}