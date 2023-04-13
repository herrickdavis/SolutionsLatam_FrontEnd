using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;

namespace AlsGlobal.Extensions
{
  public class ExceptionMiddleware
  {
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
      try
      {
      
        await _next(httpContext);
      }
      catch (CustomHttpRequestException ex)
      {
        HandleRequestExceptionAsync(httpContext, ex.StatusCode);
      }
    }

    private static void HandleRequestExceptionAsync(HttpContext context, HttpStatusCode statusCode)
    {
      if (statusCode == HttpStatusCode.Unauthorized)
      {
        context.Response.Redirect($"/?ReturnUrl={context.Request.Path}");
        return;
      }

      context.Response.StatusCode = (int)statusCode;
    }
  }
}
