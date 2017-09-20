using Microsoft.EntityFrameworkCore;
 
namespace RESTauranter.Models
{
    public class RESTauranterContext : DbContext
    {
        public RESTauranterContext(DbContextOptions<RESTauranterContext> options) : base(options) { }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Help> Helps { get; set; }
    }
}