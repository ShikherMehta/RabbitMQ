using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using InventoryService.Model;
namespace InventoryService.Data
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext(
            DbContextOptions<InventoryDbContext> options)
            : base(options)
        {
        }

        public DbSet<Inventory> Inventories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Inventory>()
                .HasKey(x => x.ProductId);
            modelBuilder.Entity<Inventory>()
                           .ToTable("Inventory");
            base.OnModelCreating(modelBuilder);
        }
    }
}