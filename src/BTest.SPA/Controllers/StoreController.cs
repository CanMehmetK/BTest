using System.Security.Claims;
using BTest.Infrastructure.Models;
using BTest.Infrastructure.Store;
using BTest.Infrastructure.Store.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BTest.SPA.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StoreController : ControllerBase
{
  private readonly IStoreService _storeService;

  public StoreController(IStoreService storeService)
  {
    _storeService = storeService;
  }

  [AllowAnonymous]
  [HttpGet("categories")]
  public async Task<BaseResponse<List<CategoryDTO>>> GetAllCategories() =>
    new BaseResponse<List<CategoryDTO>>(await _storeService.GetCategories());


  [AllowAnonymous]
  [HttpGet("products")]
  public async Task<BaseResponse<Payload<ProductDTO>>> GetProductsWithQuery([FromQuery] ProductQueryStringParameter qsParameter) =>
    new BaseResponse<Payload<ProductDTO>>(await _storeService.GetProducts(qsParameter));

  [Authorize]
  [HttpPost("create-order")]
  public async Task<BaseResponse<OrderDTO>> CreateOrder([FromBody] OrderDTO orderDto) =>
      new BaseResponse<OrderDTO>(await _storeService.CreateOrder(User, orderDto));


  [Authorize]
  [HttpPost("my-orders")]
  public async Task<BaseResponse<List<OrderDTO>>> MyOrders([FromBody] OrderFilterDTO orderFilterDto) =>
      new BaseResponse<List<OrderDTO>>(await _storeService.MyOrders(User, orderFilterDto));
}
