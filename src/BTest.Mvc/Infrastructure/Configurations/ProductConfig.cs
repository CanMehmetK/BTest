using BTest.Mvc.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BTest.Infrastructure.Configurations
{
  public class CategoryConfig : IEntityTypeConfiguration<Category>
  {
    public void Configure(EntityTypeBuilder<Category> builder)
    {
      builder.ToTable("Categories");
      builder.HasMany<Product>(g => g.Products)
      .WithOne(s => s.Category)
      .HasForeignKey(s => s.CategoryId);
    }
  }
  public class ProductConfig : IEntityTypeConfiguration<Product>
  {
    public void Configure(EntityTypeBuilder<Product> builder)
    {

      builder.ToTable("Products");

      builder.Property(t => t.Name).HasMaxLength(100);
      builder.Property(t => t.Description).HasMaxLength(500);

      builder.Property(c => c.Price).HasPrecision(16, 4);

      builder.HasOne<Stock>(s => s.Stock)
              .WithOne(ad => ad.Product)
              .HasForeignKey<Stock>(ad => ad.ProductId);



    }
  }

  public class StockConfig : IEntityTypeConfiguration<Stock>
  {
    public void Configure(EntityTypeBuilder<Stock> builder)
    {
      builder.Property(c => c.Count).HasPrecision(16, 4);

    }
  }
}

