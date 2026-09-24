using Microsoft.EntityFrameworkCore;
using OrderService.Models;
using PaymentService.Model;
using System.Reflection.Emit;

namespace PaymentService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(
            DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Payment> Payments =>
            Set<Payment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>()
                .Property(x => x.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Payment>()
                .HasIndex(x => x.OrderId)
                .IsUnique();
        }
    }
}
