using CodeChallenge.DomainLayer.Order;
using CodeChallenge.InfrastructureLayer.EventStore;
using Microsoft.EntityFrameworkCore;

namespace CodeChallenge.InfrastructureLayer;

public class OrderDbContext(DbContextOptions<OrderDbContext> options) : DbContext(options)
{
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<StoredEvent> StoredEvents { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var orderBuilder = modelBuilder.Entity<Order>();

        orderBuilder.HasKey(o => o.Id);

        orderBuilder.Property(o => o.Id)
        .ValueGeneratedOnAdd();

        orderBuilder.OwnsOne(o => o.InvoiceAddress, a =>
        {
            a.Property(p => p.Value).HasColumnName("InvoiceAddress");
        });

        orderBuilder.OwnsOne(o => o.InvoiceEmail, e =>
        {
            e.Property(p => p.Value).HasColumnName("InvoiceEmail");
        });

        orderBuilder.OwnsOne(o => o.CreditCard, c =>
        {
            c.Property(p => p.Value).HasColumnName("CreditCardNumber");
        });

        orderBuilder.OwnsMany(o => o.Items, item =>
        {
            item.WithOwner().HasForeignKey("OrderId"); 
            item.Property(i => i.ProductId).IsRequired();
            item.Property(i => i.ProductName).IsRequired();
            item.Property(i => i.ProductAmount).IsRequired();
            item.Property(i => i.ProductPrice).IsRequired().HasColumnType("decimal(18,2)");

            item.HasKey("OrderId", "ProductId"); 
        });
    }
}