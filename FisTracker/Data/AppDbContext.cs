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

            modelBuilder.Entity<User>(e =>
            {
                e.HasMany(u => u.TimeInputs).WithOne();
                e.HasKey(p => p.Id);
                e.Property(p => p.Name).HasMaxLength(83);
                e.HasIndex(p => p.Name).IsUnique();
            });

            modelBuilder.Entity<Session>(e => e.Property(p => p.Id).HasMaxLength(100));

            modelBuilder.Entity<TimeInput>(e =>
            {
                e.HasKey(p => new { p.Date, p.UserId });
            });
        }
    }
}
