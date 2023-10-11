using Microsoft.EntityFrameworkCore;
using UsersAPI.Models.Business;

namespace UsersAPI.Models.DatabaseContext
{
    public class UsersDataBaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public UsersDataBaseContext(DbContextOptions options) : base(options)
        {
            this.Database.Migrate();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
