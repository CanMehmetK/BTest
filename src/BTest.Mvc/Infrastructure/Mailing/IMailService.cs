using BTest.Mvc.Models;

namespace BTest.Infrastructure.Interfaces
{
  public interface IMailService
  {
    Task SendAsync(EMail request);
  }
}
