using Microsoft.EntityFrameworkCore;
namespace TEST2.Models
{
    public class YourDbContext : DbContext
    {
        public YourDbContext(DbContextOptions<YourDbContext> options) : base(options)
        {

        }
        
        public DbSet<Users> Users { get; set; }
    }
}
