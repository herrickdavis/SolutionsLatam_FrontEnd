using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AlsGlobal.Extensions;
using AlsGlobal.Models;
using Microsoft.AspNetCore.Http;

namespace AlsGlobal.Services{
  public interface IProyectoService
  {
    Task<IEnumerable<ProyectoResponseViewModel>> GetProyectos(ProyectoRequestViewModel request);
    Task<ResponseServiceViewModel<List<string>, List<string>, List<string>>> GetAllProyectos(FiltrosViewModel filtro, int page = 1, int rowPage = 20);
    Task<AsignarAliasResponseModel> SetAlias(AsignarAliasRequestViewModel request);
    }
  public class ProyectoService: Service, IProyectoService
  {
    private readonly HttpClient _httpClient;
    private readonly IAspNetUser _aspNetUser;
    private readonly IHttpContextAccessor _httpContext;
    public ProyectoService(HttpClient httpClient,
                                IAspNetUser aspNetUser,
                                IHttpContextAccessor httpContext)
    {
      _httpClient = httpClient;
      _aspNetUser = aspNetUser;
      _httpContext = httpContext;
    }
    public async Task<IEnumerable<ProyectoResponseViewModel>> GetProyectos(ProyectoRequestViewModel request)
    {
      var contenido = ArmarContenido(request);
      string cultura = ObtenerCultura(_httpContext);
      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
     _httpClient.DefaultRequestHeaders.Add("culture", cultura);
     _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
      var response = await _httpClient.PostAsync("GetProyectos", contenido);
      var result = await DeserializarObjetoResponse<IEnumerable<ProyectoResponseViewModel>>(response);
      return result;
    }

        public async Task<ResponseServiceViewModel<List<string>, List<string>, List<string>>> GetAllProyectos(FiltrosViewModel filtro, int page = 1, int rowPage = 20)
        {
            var contenido = ArmarContenido(filtro);
            string cultura = ObtenerCultura(_httpContext);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
            _httpClient.DefaultRequestHeaders.Add("culture", cultura); _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            var response = await _httpClient.PostAsync("GetAllProyectos?page=" + page + "&rowPage=" + rowPage, contenido);
            //var response = await _httpClient.PostAsync("GetAllEstaciones", null);

            var result = await DeserializarObjetoResponse<ResponseServiceViewModel<List<string>, List<string>, List<string>>>(response);

            return result;
        }

        public async Task<AsignarAliasResponseModel> SetAlias(AsignarAliasRequestViewModel request)
        {
            var contenido = ArmarContenido(request);
            string cultura = ObtenerCultura(_httpContext);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
            _httpClient.DefaultRequestHeaders.Add("culture", cultura);
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            var response = await _httpClient.PostAsync("SetAliasProyectos", contenido);
            var result = await DeserializarObjetoResponse<AsignarAliasResponseModel>(response);
            return result;
        }
    }
}