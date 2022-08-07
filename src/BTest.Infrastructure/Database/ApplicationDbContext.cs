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

  public DbSet<Product> Products { get; set; }
  public DbSet<Category> Categories { get; set; }


}
