using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BTest.Infrastructure.Database.Entities;
public class Product : BaseEntity
{
  public string Name { get; set; } = default!;
  public string? Slug { get; set; }
  public string Description { get; set; }
  public decimal Price { get; set; }
  public string Image { get; set; } = "noimage.png";

  public int CategoryId { get; set; }

  public Category? Category { get; set; }

  public Stock? Stock { get; set; }
  public List<OrderDetail>? OrderDetail { get; set; }
}

public class ProductConfig : IEntityTypeConfiguration<Product>
{
  public void Configure(EntityTypeBuilder<Product> builder)
  {

    builder.ToTable("Products");

    builder.Property(t => t.Name).HasMaxLength(100).IsRequired();
    builder.Property(t => t.Slug).HasMaxLength(200);
    builder.Property(t => t.Image).HasMaxLength(40);
    builder.Property(t => t.Description).HasMaxLength(500);
    builder.Property(c => c.Price).HasPrecision(16, 4);
    builder.Property(p => p.CreateUTC).HasColumnType("DateTime").HasDefaultValueSql("GetUtcDate()");

    builder.HasMany<OrderDetail>(g => g.OrderDetail)
      .WithOne(s => s.Product)
      .HasForeignKey(s => s.ProductId);

    builder.HasOne<Stock>(s => s.Stock)
              .WithOne(ad => ad.Product)
              .HasForeignKey<Stock>(ad => ad.ProductId);
  }
}
