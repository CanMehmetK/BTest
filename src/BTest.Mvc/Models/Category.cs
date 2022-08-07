namespace BTest.Mvc.Models
{
  public class Category : BaseEntity<Guid>
  {
    public string Name { get; set; }
    public string Slug { get; set; }
    public List<Product> Products { get; set; }
  }
}
