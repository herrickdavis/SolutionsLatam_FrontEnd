
using AlsGlobal.Extensions;
using AlsGlobal.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AlsGlobal.Services
{
  public interface IEstacionesService
  {
    Task<IEnumerable<EstacionesViewModel>> GetEstacionesReporteEstaciones(EstacionesReporteEstacionesRequestViewModel request);
    Task<IEnumerable<ReporteEstacionesViewModel>> GetReporteEstaciones(ReporteEstacionesRequestViewModel request);
  }
  public class EstacionesService : Service, IEstacionesService
  {
    private readonly HttpClient _httpClient;
    private readonly IAspNetUser _aspNetUser;
    private readonly IHttpContextAccessor _httpContext;
    public EstacionesService(HttpClient httpClient,
                                IAspNetUser aspNetUser,
                                IHttpContextAccessor httpContext)
    {
      _httpClient = httpClient;
      _aspNetUser = aspNetUser;
      _httpContext = httpContext;
    }
    public async Task<IEnumerable<EstacionesViewModel>> GetReporteEstaciones(EstacionesReporteEstacionesRequestViewModel request)
    {
      var contenido = ArmarContenido(request);
      string cultura = ObtenerCultura(_httpContext);
      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
     _httpClient.DefaultRequestHeaders.Add("culture", cultura);_httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
      var response = await _httpClient.PostAsync("GetEstacionesReporteEstaciones", contenido);

      var result = await DeserializarObjetoResponse<IEnumerable<EstacionesViewModel>>(response);
      return result;
    }
    public async Task<IEnumerable<EstacionesViewModel>> GetEstacionesReporteEstaciones(EstacionesReporteEstacionesRequestViewModel request)
    {
      var contenido = ArmarContenido(request);
      string cultura = ObtenerCultura(_httpContext);
      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
     _httpClient.DefaultRequestHeaders.Add("culture", cultura);_httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
      var response = await _httpClient.PostAsync("GetEstacionesReporteEstaciones", contenido);

      var result = await DeserializarObjetoResponse<IEnumerable<EstacionesViewModel>>(response);
      return result;
    }
    public async Task<IEnumerable<ReporteEstacionesViewModel>> GetReporteEstaciones(ReporteEstacionesRequestViewModel request)
    {
      var contenido = ArmarContenido(request);
      string cultura = ObtenerCultura(_httpContext);
      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
     _httpClient.DefaultRequestHeaders.Add("culture", cultura);_httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
      var response = await _httpClient.PostAsync("GetReporteEstaciones", contenido);

      var result = await DeserializarObjetoResponse<IEnumerable<ReporteEstacionesViewModel>>(response);
      return result;
    }
  }
}
