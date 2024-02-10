using Microsoft.EntityFrameworkCore;

namespace ClipUp.Database
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureDeleted();
        }
    }
}
