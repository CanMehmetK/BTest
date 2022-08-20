using AutoMapper;
using BTest.Infrastructure.Database.Entities;
using BTest.Infrastructure.Identity.DTO;
using BTest.Infrastructure.Identity.Entities;
using BTest.Infrastructure.Store.DTO;
using Microsoft.EntityFrameworkCore;

namespace BTest.Infrastructure;

public class MappingProfiles : Profile
{
  public MappingProfiles()
  {
    CreateMap<ApplicationUser, UserDto>();
    CreateMap<Category, CategoryDTO>();
    CreateMap<Product, ProductDTO>()
      .ForMember(dst => dst.Quantity, opt => opt.MapFrom(
        src => src.Stock != null ? src.Stock.Quantity : 0));

    CreateMap<ProductDTO, Product>();

    CreateMap<OrderDTO, Order>();
    CreateMap<Order, OrderDTO>();

    CreateMap<OrderDetailDTO, OrderDetail>();
    CreateMap<OrderDetail, OrderDetailDTO>()
      .ForMember(dst => dst.ProductName, opt => opt.MapFrom(
        src => src.Product.Name));
  }
}
