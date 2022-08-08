using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BTest.Infrastructure.Database.Entities;

public class Order : BaseEntity
{
  public decimal TotalValue { get; set; }
  public List<OrderDetail> OrderDetails { get; set; }
  public List<UserOrder> UserOrders { get; set; }
}

public class OrderConfig : IEntityTypeConfiguration<Order>
{
  public void Configure(EntityTypeBuilder<Order> builder)
  {
    builder.ToTable("Orders");
    builder.Property<DateTime>("CreateUTC");
    builder.Property(c => c.TotalValue).HasPrecision(18, 2);
    builder.HasMany<OrderDetail>(g => g.OrderDetails)
      .WithOne(s => s.Order)
      .HasForeignKey(s => s.OrderId);
  }
}
