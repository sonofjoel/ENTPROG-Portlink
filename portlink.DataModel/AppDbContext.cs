using System;
using Microsoft.EntityFrameworkCore;

namespace portlink.DataModel
{
    public class AppDbContext : DbContext
    {
        // Add a constructor that accepts DbContextOptions<AppDbContext>
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // DbSets for the application
        public DbSet<User> Users { get; set; }
        public DbSet<Port> Ports { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Tracking> Trackings { get; set; }

        // This DbSet for CargoRequests has been removed.

        // Remove the OnConfiguring method.
        // The configuration is now handled in Program.cs and passed through the constructor.

        // Configuring model relationships
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
