using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace BTest.Infrastructure.Identity;

public interface IAuthenticatedUserService
{
  string UserEmail { get; }
}

public class AuthenticatedUserService : IAuthenticatedUserService
{
  public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor)
  {
    UserEmail = httpContextAccessor.HttpContext?.User?
      .FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
  }

  public string UserEmail { get; }
}
