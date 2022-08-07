using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BTest.Infrastructure.Database.Entities;

public class OrderDetail : BaseEntity
{

  public decimal Quantity { get; set; }
  public decimal UnitPrice { get; set; }
  public decimal TotalPrice { get; set; }

  public int ProductId { get; set; }
  public Product Product { get; set; }


  public int OrderId { get; set; }
  public Order Order { get; internal set; }
}

public class OrderDetailConfig : IEntityTypeConfiguration<OrderDetail>
{
  public void Configure(EntityTypeBuilder<OrderDetail> builder)
  {
    builder.ToTable("OrderDetails");
    builder.Property(c => c.Quantity).HasPrecision(18, 2);
    builder.Property(c => c.UnitPrice).HasPrecision(18, 2);
    builder.Property(c => c.TotalPrice).HasPrecision(18, 2);

  }
}
