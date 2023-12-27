using CustomerManager.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerManager.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<ContactNumbers> ContactNumbers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<CustomerWithNumbersDTO>();
        }
    }
}
