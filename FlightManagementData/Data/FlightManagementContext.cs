using FlightManagementData.Models.Accounts;
using FlightManagementData.Models.Address;
using FlightManagementData.Models.Airports;
using FlightManagementData.Models.Flights;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FlightManagementData.Data
{
    public class FlightManagementContext : IdentityDbContext<User>
    {
        public FlightManagementContext(DbContextOptions<FlightManagementContext> options) : base(options) { }

        //Accounts
        public DbSet<User> User { get; set; }

        //Address
        public DbSet<City> City { get; set; }
        public DbSet<Country> Country { get; set; }

        //Airport
        public DbSet<Airport> Airport { get; set; }

        //Flight
        public DbSet<AirplaneType> AirplaneType { get; set; }
        public DbSet<Flight> Flight { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Flight>()
                .HasOne(da => da.DepartureAirpot)
                .WithMany()
                .HasForeignKey(da => da.DepartureAirpotID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Flight>()
                .HasOne(da => da.ArrivalAirport)
                .WithMany()
                .HasForeignKey(da => da.ArrivalAirportID)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
