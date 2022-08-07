using System.Security.Claims;
using BTest.Infrastructure.Database.Entities;
using BTest.Infrastructure.Store.DTO;

namespace BTest.Infrastructure.Store;
public interface IStoreService
{
  Task<List<CategoryDTO>> GetCategories();
  Task<Payload<ProductDTO>> GetProducts(ProductQueryStringParameter qsParameter);
  Task<OrderDTO> CreateOrder(ClaimsPrincipal User, OrderDTO orderDto);
}
