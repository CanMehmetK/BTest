namespace BTest.Infrastructure.Identity.DTO;

public class RefreshTokenRequest
{
  public string AccessToken { get; set; }
  public string RefreshToken { get; set; }
}
