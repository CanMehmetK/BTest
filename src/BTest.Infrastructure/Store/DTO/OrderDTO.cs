namespace BTest.Infrastructure.Store.DTO;

public class OrderDTO
{
  public int? Id { get; set; }
  public DateTime CreateUTC { get; set; }
  public decimal TotalValue { get; set; }
  public List<OrderDetailDTO> OrderDetails { get; set; }
}
