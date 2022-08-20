using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BTest.Infrastructure.Database.Entities;

public class Category : BaseEntity
{
  public string Name { get; set; }
  public string Slug { get; set; }
  public List<Product> Products { get; set; }
}

public class CategoryConfig : IEntityTypeConfiguration<Category>
{
  public void Configure(EntityTypeBuilder<Category> builder)
  {
    builder.ToTable("Categories");
    builder.Property(t => t.Name).HasMaxLength(100).IsRequired();
    builder.Property(t => t.Slug).HasMaxLength(200);
    builder.Property(p => p.CreateUTC).HasColumnType("DateTime");
    builder.Property(p => p.UpdateUTC).HasColumnType("DateTime");
    builder.HasMany<Product>(g => g.Products)
    .WithOne(s => s.Category)
    .HasForeignKey(s => s.CategoryId);
  }
}
