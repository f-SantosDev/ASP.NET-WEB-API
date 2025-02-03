using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Revisao_ASP.NET_Web_API.Models.Entities;

namespace Revisao_ASP.NET_Web_API.Models
{
    //public class AppDbContext : DbContext // create a database context without identity user
    public class AppDbContext : IdentityDbContext<AppUser> // create a database context with identity user
    {
        // define DbContext through class constructor / string connection, database tipe < SQLSERVER >, etc
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // representation of database tables
        public DbSet<Clients> Clients { get; set; }
        public DbSet<Reservations> Reservations { get; set; }

        // this method sets the configuration for database table
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // implement the bases of method OnModelCreating
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Clients>()
                .HasKey(c => c.ClientId);

            modelBuilder.Entity<Reservations>()
                .HasKey(r => r.ReservationId);

            modelBuilder.Entity<Reservations>()
                .HasOne(r => r.Client) // navigation property from Reservations class - define that Reservations has relation with least one client
                .WithMany(c => c.Reservations) // defines that the Client has a relationship with many reservations
                .HasForeignKey(r => r.ClientId) // sets the ClientId property as foreignKey in the Reservations table
                .OnDelete(DeleteBehavior.Cascade); // especifies that all reservations associated with the client will also be deleted
        }
    }
}
