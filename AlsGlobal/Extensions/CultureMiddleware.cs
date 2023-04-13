using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System.Threading.Tasks;

namespace AlsGlobal.Extensions
{
  public class CultureMiddleware
  {
    private readonly RequestDelegate _next;
    public CultureMiddleware(RequestDelegate next)
    {
      _next = next;
    }
    public async Task InvokeAsync(HttpContext httpContext)
    {
      string culture = TransformQueryToCookieValue(httpContext, httpContext.Request.Query["culture"]);
      SetCookieValue(httpContext, culture);
      await _next(httpContext);
    }
    private string TransformQueryToCookieValue(HttpContext httpContext, string culture)
    {
      switch (culture)
      {
        case "es-MX":
        case "es-PE":
          culture = "c=es-PE|uic=es-PE";break;
        case "en-US":
          culture = "c=en-US|uic=en-US";break;
        case "pt-BR":
          culture = "c=pt-BR|uic=pt-BR";break;
        default: culture = GetCookieValue(httpContext);break;
      }
      return culture;
    }

    private string GetCookieValue(HttpContext httpContext)
    {
      var cookieName = CookieRequestCultureProvider.DefaultCookieName;
      return httpContext.Request.Cookies[cookieName] ?? "c=es-PE|uic=es-PE";
    }

    private void SetCookieValue(HttpContext httpContext, string culture)
    {
      httpContext.Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, culture);
    }
  }
}
