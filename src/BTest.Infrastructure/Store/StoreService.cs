using System.Reflection;
using System.Security.Claims;
using AutoMapper;
using BTest.Infrastructure.Database;
using BTest.Infrastructure.Database.Entities;
using BTest.Infrastructure.Store.DTO;
using Microsoft.EntityFrameworkCore;

namespace BTest.Infrastructure.Store;

public class StoreService : IStoreService
{
  public static string ProductImagePath => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "wwwroot", "images", "products");
  private readonly IMapper _mapper;
  private readonly IGenericRepository<Category> _crepository;
  private readonly IGenericRepository<Product> _prepository;
  private readonly IGenericRepository<Order> _orepository;
  private readonly IUnitOfWork _unitOfWork;
  public StoreService(
    IGenericRepository<Category> crepository,
    IGenericRepository<Product> prepository,
    IGenericRepository<Order> orepository,
    IUnitOfWork unitOfWork,
    IMapper mapper)
  {
    _crepository = crepository;
    _prepository = prepository;
    _orepository = orepository;
    _unitOfWork = unitOfWork;
    _mapper = mapper;
  }
  public async Task<List<CategoryDTO>> GetCategories() =>
     _mapper.Map<List<Category>, List<CategoryDTO>>(await _crepository.GetAll());

  public async Task<Payload<ProductDTO>> GetProducts(ProductQueryStringParameter qsParameter)
  {
    IQueryable<Product> query = _prepository.context.Products
      .Include(p => p.Stock)
      .OrderBy(t => t.Id);

    if (qsParameter.Id != null && qsParameter.Id > 0)
      query = query.Where(item => item.Id == qsParameter.Id);

    if (!string.IsNullOrEmpty(qsParameter.Name))
      query = query.Where(item => item.Name.ToLower().Contains(qsParameter.Name.Trim().ToLower()));

    if (qsParameter.Categories != null && qsParameter.Categories.Count > 0)
      query = query.Where(p => qsParameter.Categories.Contains(p.CategoryId));

    int count = await query.CountAsync();

    query = query
      .Skip((qsParameter.PageNumber - 1) * qsParameter.PageSize)
      .Take(qsParameter.PageSize);
    List<ProductDTO> list = _mapper.Map<List<Product>, List<ProductDTO>>(await query.ToListAsync());


    return new Payload<ProductDTO>(list, count);
  }

  public async Task<OrderDTO> CreateOrder(ClaimsPrincipal User, OrderDTO orderDto)
  {
    var userid = int.Parse(User.FindFirstValue("uid"));

    var order = _mapper.Map<OrderDTO, Order>(orderDto);
    order.UserOrders = new List<UserOrder>() { new UserOrder() { UserId = userid } };

    // Check Stock Amount ... Available ...

    var newOrder = _mapper.Map<Order, OrderDTO>(await _orepository.InsertAsync(order));

    // Set Stock Amount

    // Email service send Order notify email...

    return newOrder;
  }

  public async Task<List<OrderDTO>> MyOrders(ClaimsPrincipal User, OrderFilterDTO orderFilterDto)
  {
    var userid = int.Parse(User.FindFirstValue("uid"));
    var query = _orepository.context.Set<UserOrder>()
      .Where(t => t.UserId == userid)
      .Include(uo => uo.Order)
      .ThenInclude(o => o.OrderDetails)
      .ThenInclude(o => o.Product)
      .Select(t => t.Order)
      ;
    var result = _mapper.Map<List<Order>, List<OrderDTO>>(await query.ToListAsync());
    return result;
  }

  public async Task<int> CreateProduct(ProductDTO productDTO)
  {
    var fileName = "noimage.png";
    var imageData = productDTO.Image.Contains(";base64,") ? productDTO.Image.Split(";base64,")[1] : null;
    if (imageData != null && imageData.Length > 0)
    {
      fileName = Guid.NewGuid().ToString() + ".jpg";
      var streamData = new MemoryStream(Convert.FromBase64String(imageData));
      using var stream = new FileStream(Path.Combine(ProductImagePath, fileName), FileMode.Create);
      await streamData.CopyToAsync(stream);

    }
    productDTO.Image = fileName;

    var product = await _prepository.InsertAsync(_mapper.Map<Product>(productDTO));
    return product.Id;
  }

  public async Task<int> UpdateProduct(ProductDTO productDTO)
  {
    Product product = await _prepository.context.Products.AsNoTracking()
      .FirstAsync(item => item.Id == productDTO.Id);
    
    var fileName = "noimage.png";
    var imageData = productDTO.Image.Contains(";base64,") ? productDTO.Image.Split(";base64,")[1] : null;
    if (imageData != null && imageData.Length > 0)
    {
      fileName = product.Image == "noimage.png" ? Guid.NewGuid().ToString() + ".jpg" : product.Image;
      var streamData = new MemoryStream(Convert.FromBase64String(imageData));
      using var stream = new FileStream(Path.Combine(ProductImagePath, fileName), FileMode.Create);
      await streamData.CopyToAsync(stream);

    }
    productDTO.Image = fileName;
    var m = _mapper.Map<Product>(productDTO);
    m.CreateUTC = product.CreateUTC;
    m.UpdateUTC = product.UpdateUTC;
    var uproduct = await _prepository.UpdateAsync(m);
    return uproduct.Id;
  }


}
