using AutoMapper;
using BTest.Infrastructure.Database.Entities;
using BTest.Infrastructure.Identity.DTO;
using BTest.Infrastructure.Identity.Entities;
using BTest.Infrastructure.Store.DTO;

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
    CreateMap<OrderDTO, Order>().ReverseMap();
    CreateMap<OrderDetailDTO, OrderDetail>().ReverseMap();
  }
}
