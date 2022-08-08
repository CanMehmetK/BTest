using System.ComponentModel.DataAnnotations;
using BTest.Infrastructure.Database.Entities;
using Microsoft.AspNetCore.Identity;

namespace BTest.Infrastructure.Identity.Entities;

public class ApplicationUser : IdentityUser<int>
{
  [MaxLength(150)]
  public string? FirstName { get; set; }
  [MaxLength(150)]
  public string? LastName { get; set; }

  public List<UserOrder> UserOrders { get;set;}
}
