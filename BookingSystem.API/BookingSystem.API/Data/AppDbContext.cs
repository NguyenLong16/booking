using BookingSystem.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Service> Services { get; set; }
    }
}
