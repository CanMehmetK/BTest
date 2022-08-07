using BTest.Mvc.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BTest.Infrastructure;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>, ApplicationRoleClaim, IdentityUserToken<string>>
{
  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
      : base(options)
  {

  }

  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);
    builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
  }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    base.OnConfiguring(optionsBuilder);
  }

  public DbSet<Product> Products { get; set; }
  public DbSet<Category> Categories { get; set; }

}
