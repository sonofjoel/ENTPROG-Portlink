using System;
using System.Collections.Generic; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace portlink.DataModel
{
    public class AppDbContext : DbContext
    {
      
        public DbSet<User> Users { get; set; }
        public DbSet<Port> Ports { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Tracking> Tracking { get; set; }

      
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=ADMIN\\SQLEXPRESS;" +
                                        "Database=Portlink_Entprog;Integrated Security=SSPI;" +
                                        "TrustServerCertificate=true");
        }

       
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
