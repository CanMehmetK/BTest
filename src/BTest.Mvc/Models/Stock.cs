namespace BTest.Mvc.Models
{
  public class Stock : BaseEntity<Guid>
  {
    public decimal Count { get; set; }
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
  }
}
