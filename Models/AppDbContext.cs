using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Klienci> Klienci { get; set; }
    }
}
