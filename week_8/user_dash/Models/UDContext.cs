using Microsoft.EntityFrameworkCore;

namespace user_dash.Models
{
    public class UDContext : DbContext
    {
        public UDContext(DbContextOptions<UDContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}