using COMP_2001_Report.Models;
using Microsoft.EntityFrameworkCore;

namespace COMP_2001_Report.Data
{
    public class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)

    {
        public DbSet<Users> Users { get; set; }
        public DbSet<Archive_User> ArchiveUser { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().ToTable("User", "CW2").HasKey(u => u.user_id);
            modelBuilder.Entity<Archive_User>().ToTable("Archive_User", "CW2").HasKey(u => u.archive_user_id);
        }

    }

}
