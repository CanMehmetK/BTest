using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BTest.Infrastructure.Database.Entities;

public class Stock : BaseEntity
{
  public decimal Quantity { get; set; }
  public int ProductId { get; set; }
  public Product Product { get; set; }
}

public class StockConfig : IEntityTypeConfiguration<Stock>
{
  public void Configure(EntityTypeBuilder<Stock> builder)
  {
    builder.ToTable("ProductStocks");
    builder.Property(c => c.Quantity).HasPrecision(18, 2);
  }
}
