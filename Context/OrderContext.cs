using Microsoft.EntityFrameworkCore;
using OrderProcessingMVC.Models;

namespace OrderProcessingMVC.Context
{
    public class DateBaseOrderContext : DbContext
    {
        public DateBaseOrderContext(DbContextOptions<DateBaseOrderContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasOne<Provider>(p => p.Provider)
                .WithMany(o => o.Orders)
                .HasForeignKey(o => o.ProviderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .IsRequired();

            modelBuilder.Entity<OrderItem>()
                .HasOne<Order>(o => o.Order)
                .WithMany(t => t.OrderItem)
                .HasForeignKey(t => t.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .IsRequired();
        }

    }
}
