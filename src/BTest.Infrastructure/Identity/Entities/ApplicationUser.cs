using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BTest.Infrastructure.Identity.Entities;

public class ApplicationUser : IdentityUser<int>
{
  [MaxLength(150)]
  public string? FirstName { get; set; }
  [MaxLength(150)]
  public string? LastName { get; set; }
}
