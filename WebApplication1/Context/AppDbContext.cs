using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Context
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }

        public DbSet<Vehicles> Vehicles { get; set; }

        public DbSet<Tutorial> Tutorial { get; set; } 

        public DbSet<Tutorials> Tutorials { get; set; }

        public DbSet<VehicleType> vehicleType { get; set; }

        public DbSet<Fuel> fuel { get; set; }

        public DbSet<Approve> Approve { get; set; }

        public DbSet<Customer> Customer { get; set; } 




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("users");   
        }

    }
}
