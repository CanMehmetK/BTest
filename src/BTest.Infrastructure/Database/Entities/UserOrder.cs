using BTest.Infrastructure.Identity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BTest.Infrastructure.Database.Entities;

public class UserOrder
{
  public int UserId { get; set; }
  public ApplicationUser ApplicationUser { get; set; }
  public int OrderId { get; set; }
  public Order Order { get; set; }
}

public class UserOrderConfig : IEntityTypeConfiguration<UserOrder>
{
  public void Configure(EntityTypeBuilder<UserOrder> builder)
  {
    builder.ToTable("UserOrder");
    builder.HasKey(bc => new { bc.UserId, bc.OrderId });

    builder.HasOne<ApplicationUser>(bc => bc.ApplicationUser)
        .WithMany(m=>m.UserOrders)
        .HasForeignKey(bc => bc.UserId);

    builder.HasOne<Order>(bc => bc.Order)
        .WithMany(t=>t.UserOrders)
        .HasForeignKey(bc => bc.OrderId);
  }
}
