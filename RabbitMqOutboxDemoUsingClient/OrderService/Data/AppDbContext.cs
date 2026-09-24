using Microsoft.EntityFrameworkCore;
using OrderService.Models;
using System.Reflection.Emit;

namespace OrderService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(
        DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        public DbSet<Order> Orders => Set<Order>();

        public DbSet<OutboxMessage> OutboxMessages =>
            Set<OutboxMessage>();
        
    }
}
