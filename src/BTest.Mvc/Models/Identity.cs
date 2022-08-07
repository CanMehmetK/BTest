using Microsoft.AspNetCore.Identity;

namespace BTest.Mvc.Models
{
  public class ApplicationUser : IdentityUser
  {
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public bool IsActive { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    public string? ObjectId { get; set; }
  }
  public class ApplicationRole : IdentityRole
  {
    public string? Description { get; set; }

    public ApplicationRole(string name, string? description = null)
        : base(name)
    {
      Description = description;
      NormalizedName = name.ToUpperInvariant();
    }
  }

  public class ApplicationRoleClaim : IdentityRoleClaim<string>
  {

  }
}
