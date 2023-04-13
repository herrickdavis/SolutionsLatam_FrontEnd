using AlsGlobal.Extensions;
using AlsGlobal.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AlsGlobal.Services
{
    public interface IReporteHistoricoService
  {
    Task<IEnumerable<EstacionesViewModel>> GetEstaciones(EstacionesRequestViewModel request);
    Task<IEnumerable<ParametrosViewModel>> GetParametros(ParametrosRequestViewModel request);
    Task<ReporteHistoricoViewModel> GetGrafico(ReporteHistoricoRequestViewModel request);
    Task<ArchivoViewModel> GetDataHistoricaExcel(ReporteHistoricoRequestViewModel request);
  }
  public class ReporteHistoricoService : Service, IReporteHistoricoService
  {
    private readonly HttpClient _httpClient;
    private readonly IAspNetUser _aspNetUser;
    private readonly IHttpContextAccessor _httpContext;
    public ReporteHistoricoService(HttpClient httpClient,
                                IAspNetUser aspNetUser,
                                IHttpContextAccessor httpContext)
    {
      _httpClient = httpClient;
      _aspNetUser = aspNetUser;
      _httpContext = httpContext;
    }
    public async Task<IEnumerable<EstacionesViewModel>> GetEstaciones(EstacionesRequestViewModel request)
    {
      var contenido = ArmarContenido(request);
      string cultura = ObtenerCultura(_httpContext);
      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
     _httpClient.DefaultRequestHeaders.Add("culture", cultura);_httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
      var response = await _httpClient.PostAsync("GetEstaciones", contenido);


      var result = await DeserializarObjetoResponse<IEnumerable<EstacionesViewModel>>(response);
      return result;
    }
    public async Task<IEnumerable<ParametrosViewModel>> GetParametros(ParametrosRequestViewModel request)
    {
      var contenido = ArmarContenido(request);
      string cultura = ObtenerCultura(_httpContext);
      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
     _httpClient.DefaultRequestHeaders.Add("culture", cultura);_httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
      var response = await _httpClient.PostAsync("GetParametros", contenido);


      var result = await DeserializarObjetoResponse<IEnumerable<ParametrosViewModel>>(response);
      return result;
    }
    public async Task<ReporteHistoricoViewModel> GetGrafico(ReporteHistoricoRequestViewModel request)
    {
      var contenido = ArmarContenido(request);
      string cultura = ObtenerCultura(_httpContext);
      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
     _httpClient.DefaultRequestHeaders.Add("culture", cultura);_httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
      var response = await _httpClient.PostAsync("GetDataHistorica", contenido);
      var result = await DeserializarObjetoResponse<ReporteHistoricoViewModel>(response);
      return result;

    }
    public async Task<ArchivoViewModel> GetDataHistoricaExcel(ReporteHistoricoRequestViewModel request)
    {
      var contenido = ArmarContenido(request);
      string cultura = ObtenerCultura(_httpContext);
      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
     _httpClient.DefaultRequestHeaders.Add("culture", cultura);_httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
      var response = await _httpClient.PostAsync("GetDataHistoricaExcel", contenido);
      if (!TratarError(response))
      {
        return null;
      }
      var result = await DeserializarArchivoResponse(response);
      return new ArchivoViewModel(response.Content.Headers.ContentType.MediaType.ToString(), response.Content.Headers.ContentDisposition.FileName, result);
    }
  }
}
