namespace BTest.Infrastructure.Identity.DTO;

public class UserDto
{
  public string UserName { get; set; }
  public string FirstName { get; set; }
  public string LastName { get; set; }
  public string Email { get; set; }
  public List<string> Roles { get; set; }
}
