using AlsGlobal.Extensions;
using AlsGlobal.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AlsGlobal.Services
{
  
  public interface IEmpresaService
  {
    Task<IEnumerable<EmpresaViewModel>> Obtener();
    Task<EmpresaResponseViewModel> CambiarEmpresa(int id);
  }
  public class EmpresaService : Service, IEmpresaService
  {
    private readonly HttpClient _httpClient;
    private readonly IAspNetUser _aspNetUser;
    private readonly IHttpContextAccessor _httpContext;
    public EmpresaService(HttpClient httpClient,
                                IAspNetUser aspNetUser,
                                IHttpContextAccessor httpContext)
    {
      _httpClient = httpClient;
      _aspNetUser = aspNetUser;
      _httpContext = httpContext;
    }
    public async Task<IEnumerable<EmpresaViewModel>> Obtener()
    {
      var contenido = ArmarContenido(new { });
      string cultura = ObtenerCultura(_httpContext);
      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
     _httpClient.DefaultRequestHeaders.Add("culture", cultura);_httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
      var response = await _httpClient.PostAsync("GetEmpresas", contenido);

      var result = await DeserializarObjetoResponse<IEnumerable<EmpresaViewModel>>(response);
      return result;
    }

    public async Task<EmpresaResponseViewModel> CambiarEmpresa(int id)
    {
      var contenido = ArmarContenido(new { id_empresa = id});
      string cultura = ObtenerCultura(_httpContext);
      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
     _httpClient.DefaultRequestHeaders.Add("culture", cultura);_httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
      var response = await _httpClient.PostAsync("SetCambiarEmpresa", contenido);

      var result = await DeserializarObjetoResponse<EmpresaResponseViewModel>(response);
      return result;
    }
    
  }
}
