using System.Net;
using System.Text.Json;
using BTest.Infrastructure.Models;


namespace BTest.SPA.Middlewares;

public class ErrorHandlerMiddleware
{
  private readonly RequestDelegate _next;

  public ErrorHandlerMiddleware(RequestDelegate next)
  {
    _next = next;
  }

  public async Task Invoke(HttpContext context)
  {
    try
    {
      await _next(context);
    }
    catch (Exception error)
    {
      var response = context.Response;
      response.ContentType = "application/json";
      var responseModel = new BaseResponse<string>()
      {
        Succeeded = false,
        Message = string.IsNullOrEmpty(error.Message) ? "" : error.Message
      };

      switch (error)
      {
        case ApiException e:
          // custom application error
          response.StatusCode = e.StatusCode;
          break;
        case KeyNotFoundException e:
          // not found error
          response.StatusCode = (int)HttpStatusCode.NotFound;
          break;
        default:
          // unhandled error
          response.StatusCode = (int)HttpStatusCode.InternalServerError;
          break;
      }
      var result = JsonSerializer.Serialize(responseModel);

      await response.WriteAsync(result);
    }
  }
}
