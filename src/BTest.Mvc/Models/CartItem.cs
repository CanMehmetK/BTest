namespace BTest.Mvc.Models
{
  public class CartItem : BaseEntity<Guid>
  {
    public CartItem() { }
    public Guid ProductId { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Total { get { return (decimal)Quantity * Price; } }

    public string Image { get; set; }



    public CartItem(Product product)
    {
      ProductId = product.Id;
      Price = product.Price;
      Name = product.Name;
      Quantity = 1;
      Image = product.Image;

    }
  }
}
