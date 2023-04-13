using AlsGlobal.Extensions;
using AlsGlobal.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AlsGlobal.Services
{
  public interface IFueraLimiteService
  {
    Task<IEnumerable<FueraDeLimiteViewModel>> GetReporteFueraLimites(FueraDeLimiteRequestViewModel request);
    Task<ArchivoViewModel> GetReporteFueraLimiteExcel(FueraDeLimiteRequestViewModel request);
  }
  public class FueraLimiteService : Service, IFueraLimiteService
  {
    private readonly HttpClient _httpClient;
    private readonly IAspNetUser _aspNetUser;
    private readonly IHttpContextAccessor _httpContext;
    public FueraLimiteService(HttpClient httpClient,
                                IAspNetUser aspNetUser,
                                IHttpContextAccessor httpContext)
    {
      _httpClient = httpClient;
      _aspNetUser = aspNetUser;
      _httpContext = httpContext;
    }
    public async Task<IEnumerable<FueraDeLimiteViewModel>> GetReporteFueraLimites(FueraDeLimiteRequestViewModel request)
    {
      var contenido = ArmarContenido(request);
      string cultura = ObtenerCultura(_httpContext);
      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
     _httpClient.DefaultRequestHeaders.Add("culture", cultura);_httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
      var response = await _httpClient.PostAsync("GetReporteFueraLimites", contenido);
      var result = await DeserializarObjetoResponse<IEnumerable<FueraDeLimiteViewModel>>(response);
      return result;

    }
    public async Task<ArchivoViewModel> GetReporteFueraLimiteExcel(FueraDeLimiteRequestViewModel request)
    {
      var contenido = ArmarContenido(request);
      string cultura = ObtenerCultura(_httpContext);
      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
     _httpClient.DefaultRequestHeaders.Add("culture", cultura);_httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
      var response = await _httpClient.PostAsync("GetReporteFueraLimiteExcel", contenido);
      if (!TratarError(response))
      {
        return null;
      }
      var result = await DeserializarArchivoResponse(response);
      return new ArchivoViewModel(response.Content.Headers.ContentType.MediaType.ToString(), response.Content.Headers.ContentDisposition.FileName, result);
    }
  }
}
