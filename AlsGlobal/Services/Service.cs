using AlsGlobal.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AlsGlobal.Services
{
  public class Service
  {
    protected StringContent ArmarContenido(object model = null)
    {
      return new StringContent(model == null ? "": JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
    }
    protected async Task<T> DeserializarObjetoResponse<T>(HttpResponseMessage responseMessage)
    {
      var options = new JsonSerializerOptions
      {
        PropertyNameCaseInsensitive = false
      };
      var result = await responseMessage.Content.ReadAsStringAsync();
      return JsonSerializer.Deserialize<T>(result, options);
    }
    protected async Task<Stream> DeserializarArchivoResponse(HttpResponseMessage responseMessage)
    {
      var result = await responseMessage.Content.ReadAsStreamAsync();
      return result;
    }
    protected bool TratarError(HttpResponseMessage response)
    {
      switch ((int)response.StatusCode)
      {
        case 401:
        case 403:
        case 404:
        case 500:
          throw new CustomHttpRequestException(response.StatusCode);

        case 400:
          return false;
      }

      response.EnsureSuccessStatusCode();
      return true;
    }
    protected string ObtenerCultura(IHttpContextAccessor httpContext)
    {
      string culture = httpContext.HttpContext.Request.Query["culture"];
      switch (culture)
      {
        case "es-MX":
        case "es-PE":
          culture = "c=es-PE|uic=es-PE"; break;
        case "en-US":
          culture = "c=en-US|uic=en-US"; break;
        case "pt-BR":
          culture = "c=pt-BR|uic=pt-BR"; break;
        default: culture = httpContext.HttpContext.Request.Cookies[CookieRequestCultureProvider.DefaultCookieName] ?? "c=es-PE|uic=es-PE"; break;
      }
      return culture;
    }
  }
}
