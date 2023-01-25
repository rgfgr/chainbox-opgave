using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Model
{
    public class CykleContext : DbContext
    {
        public CykleContext(DbContextOptions<CykleContext> options) : base(options) { }

        public DbSet<Member> Members { get; set; }
        public DbSet<Image> Images { get; set; }
    }
}
