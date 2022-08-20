using BTest.Infrastructure.Database.Entities;
using BTest.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BTest.Infrastructure.Database;

public partial class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
{
  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
  {
  }
  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);
    // TypeConfigs...
    builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
  }
  public override int SaveChanges()
  {
    setDates();

    return base.SaveChanges();
  }
  public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    setDates();
    return base.SaveChangesAsync(cancellationToken);
  }

  private void setDates()
  {
    var entries = ChangeTracker
    .Entries()
    .Where(e => e.Entity is BaseEntity && (
            e.State == EntityState.Added
            || e.State == EntityState.Modified));

    foreach (var entityEntry in entries)
    {
      ((BaseEntity)entityEntry.Entity).UpdateUTC = DateTime.UtcNow;

      if (entityEntry.State == EntityState.Added)
      {
        ((BaseEntity)entityEntry.Entity).CreateUTC = DateTime.UtcNow;
      }
    }

  }

  public DbSet<Product> Products { get; set; }
  public DbSet<Category> Categories { get; set; }

  public DbSet<Order> Orders { get; set; }
  public DbSet<OrderDetail> OrderDetails { get; set; }

}
