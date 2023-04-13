using AlsGlobal.Extensions;
using AlsGlobal.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AlsGlobal.Services
{
  public interface IParametrosService
  {
    Task<IEnumerable<ParametrosViewModel>> GetParametrosReporteEstaciones(ParametrosReporteEstacionRequestViewModel request);
  }
  public class ParametrosService : Service, IParametrosService
  {
    private readonly HttpClient _httpClient;
    private readonly IAspNetUser _aspNetUser;
    private readonly IHttpContextAccessor _httpContext;
    public ParametrosService(HttpClient httpClient,
                                IAspNetUser aspNetUser,
                                IHttpContextAccessor httpContext)
    {
      _httpClient = httpClient;
      _aspNetUser = aspNetUser;
      _httpContext = httpContext;
    }
    public async Task<IEnumerable<ParametrosViewModel>> GetParametrosReporteEstaciones(ParametrosReporteEstacionRequestViewModel request)
    {
      var contenido = ArmarContenido(request);
      string cultura = ObtenerCultura(_httpContext);
      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
     _httpClient.DefaultRequestHeaders.Add("culture", cultura);_httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
      var response = await _httpClient.PostAsync("GetParametrosReporteEstaciones", contenido);

      var result = await DeserializarObjetoResponse<IEnumerable<ParametrosViewModel>>(response);
      return result;
    }
  }
}
