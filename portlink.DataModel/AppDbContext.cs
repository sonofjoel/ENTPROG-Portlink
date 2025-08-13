using Microsoft.EntityFrameworkCore;
using portlink.DataModel;

namespace portlink.DataModel
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Port> Ports { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Tracking> Trackings { get; set; }
      

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Tracking)
                .WithOne(t => t.Booking)
                .HasForeignKey<Tracking>(t => t.BookingID);

            base.OnModelCreating(modelBuilder);
        }
    }
}
