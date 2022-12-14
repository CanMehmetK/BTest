using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTest.Infrastructure.Store.DTO;
public class ProductQueryStringParameter
{
  const int maxPageSize = 50;
  public int PageNumber { get; set; } = 1;
  private int _pageSize = 10;
  public int PageSize
  {
    get { return _pageSize; }
    set { _pageSize = (value > maxPageSize) ? maxPageSize : value; }
  }
  public string? Name { get; set; }
  public int? Id { get; set; }
  public List<int>? Categories { get; set; }
}
