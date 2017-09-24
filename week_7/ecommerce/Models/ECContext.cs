using Microsoft.EntityFrameworkCore;

namespace ecommerce.Models
{
    public class ECContext : DbContext
    {
        public ECContext(DbContextOptions<ECContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}