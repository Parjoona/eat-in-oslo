using Microsoft.EntityFrameworkCore;

namespace EatInOslo.Models
{
    public class EatInOsloContext : DbContext
    {
        public EatInOsloContext (DbContextOptions<EatInOsloContext> options)
            : base(options)
            {
            }

        public DbSet<Restaurant> Restaurant { get; set; }
        public DbSet<Review> Review { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Image> Image { get; set; }
    }

}