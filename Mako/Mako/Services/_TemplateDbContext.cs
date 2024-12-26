using Mako.Infrastructure;
using Mako.Services.Shared;
using Microsoft.EntityFrameworkCore;

namespace Mako.Services
{
    public class MakoDbContext : DbContext
    {
        public MakoDbContext()
        {
        }

        public MakoDbContext(DbContextOptions<MakoDbContext> options) : base(options)
        {
            DataGenerator.InitializeUsers(this);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Worker> Workers { get; set; }
    }
}
