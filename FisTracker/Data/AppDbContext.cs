using Microsoft.EntityFrameworkCore;

namespace FisTracker.Data
{
    public class AppDbContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<TimeInput> TimeInputs { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public AppDbContext(DbContextOptions opts) : base(opts) { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Session>()
                .HasOne(p => p.User)
                .WithMany();

            modelBuilder.Entity<User>().HasMany(u => u.TimeInputs).WithOne();

            modelBuilder.Entity<Session>(e => e.Property(p => p.Id).HasMaxLength(100));
        }
    }
}
