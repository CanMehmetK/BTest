namespace BTest.Infrastructure.Database.Entities;


public abstract class BaseEntity
{
  public int Id { get; set; }
  public DateTime CreateUTC { get; set; }
}
