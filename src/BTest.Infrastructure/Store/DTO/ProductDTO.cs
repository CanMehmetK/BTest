namespace BTest.Infrastructure.Store.DTO;

public class ProductDTO
{
  public int Id { get; set; }
  public string Name { get; set; } = default!;
  public string? Slug { get; set; }
  public string Description { get; set; }
  public decimal Price { get; set; }
  public string Image { get; set; }
  public int CategoryId { get; set; }
  public decimal Quantity { get; set; }
}


public class Payload<T>
{
  public List<T> Items { get; set; }
  public int Count { get; set; }

  public Payload(List<T> Items, int Count)
  {
    this.Items = Items;
    this.Count = Count;
  }
}
