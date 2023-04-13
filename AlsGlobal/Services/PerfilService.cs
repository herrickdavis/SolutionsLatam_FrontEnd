using AlsGlobal.Extensions;
using AlsGlobal.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AlsGlobal.Services
{
  public interface IPerfilService
  {
    Task<CambiarPasswordResponseViewModel> CambiarPassword(CambiarPasswordRequestViewModel request);
    Task<ColumnasUsuarioResponseViewModel> SetColumnasUsuario(ColumnasUsuarioRequestViewModel request);
    Task<IEnumerable<ColumnasResponseViewModel>> GetColumnas(ColumnasRequestViewModel request);
  }
  public class PerfilService: Service, IPerfilService
  {
    private readonly HttpClient _httpClient;
    private readonly IAspNetUser _aspNetUser;
    private readonly IHttpContextAccessor _httpContext;
    public PerfilService(HttpClient httpClient,
                                IAspNetUser aspNetUser,
                                IHttpContextAccessor httpContext)
    {
      _httpClient = httpClient;
      _aspNetUser = aspNetUser;
      _httpContext = httpContext;
    }
    public async Task<CambiarPasswordResponseViewModel> CambiarPassword(CambiarPasswordRequestViewModel request)
    {
      request.email = _aspNetUser.Email.ToLower();
      var contenido = ArmarContenido(request);
      string cultura = ObtenerCultura(_httpContext);
      var response = await _httpClient.PostAsync("GetCambiarPassword", contenido);
      var result = await DeserializarObjetoResponse<CambiarPasswordResponseViewModel>(response);
      result.success = response.StatusCode == System.Net.HttpStatusCode.OK;
      return result;
    }
    public async Task<ColumnasUsuarioResponseViewModel> SetColumnasUsuario(ColumnasUsuarioRequestViewModel request){
      var contenido = ArmarContenido(request);
      string cultura = ObtenerCultura(_httpContext);
      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
      _httpClient.DefaultRequestHeaders.Add("culture", cultura);
      var response = await _httpClient.PostAsync("SetColumnasUsuario", contenido);
      var result = await DeserializarObjetoResponse<ColumnasUsuarioResponseViewModel>(response);
      return result;
    }
    public async Task<IEnumerable<ColumnasResponseViewModel>> GetColumnas(ColumnasRequestViewModel request){
      var contenido = ArmarContenido(request);
      string cultura = ObtenerCultura(_httpContext);
      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
      _httpClient.DefaultRequestHeaders.Add("culture", cultura);
      var response = await _httpClient.PostAsync("GetColumnas", contenido);
      if (response.StatusCode != HttpStatusCode.OK)
      {
        return new List<ColumnasResponseViewModel>();
      }
      var result = await DeserializarObjetoResponse<IEnumerable<ColumnasResponseViewModel>>(response);
      return result;
    }
    
  }
}
