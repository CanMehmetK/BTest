namespace BTest.Infrastructure.Store.DTO;

public class CategoryDTO
{
  public int Id { get; set; }
  public DateTime CreateUTC { get; set; }
  public string Name { get; set; }
  public string Slug { get; set; }
}
