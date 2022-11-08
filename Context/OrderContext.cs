using Microsoft.EntityFrameworkCore;
using OrderProcessingMVC.Models;

namespace OrderProcessingMVC.Context
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options)
            : base(options)
        {
            this.Database.EnsureCreated();
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasOne<Provider>(p => p.Provider)
                .WithOne(o => o.Order)
                .HasForeignKey<Order>(o=>o.ProviderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .IsRequired();

            modelBuilder.Entity<OrderItem>()
                .HasOne<Order>(o => o.Order)
                .WithOne(t => t.OrderItem)
                .HasForeignKey<OrderItem>(t => t.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .IsRequired();
        }
    }
}
