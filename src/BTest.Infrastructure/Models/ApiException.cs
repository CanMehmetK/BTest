using System.Globalization;

namespace BTest.Infrastructure.Models;

public class ApiException : Exception
{
  public ApiException() : base() { }

  public ApiException(string message) : base(message) { }

  public ApiException(string message, params object[] args)
      : base(String.Format(CultureInfo.CurrentCulture, message, args))
  {
   
  } 
  public int StatusCode { get; set; }
 
}
