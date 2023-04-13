using AlsGlobal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using System.Net.Http;
using System.Threading.Tasks;

namespace AlsGlobal.Services
{
  public interface IAutenticacionService
  {
    Task<UsuarioRespuestaViewModel> Login(LoginViewModel loginViewModel);
  }
  public class AutenticacionService : Service, IAutenticacionService
  {
    private readonly HttpClient _httpClient;
    private readonly IStringLocalizer<SharedResources> _sharedLocalizer;
    private readonly IHttpContextAccessor _httpContext;
    public AutenticacionService(HttpClient httpClient,
                              IStringLocalizer<SharedResources> sharedLocalizer,
                                IHttpContextAccessor httpContext)
    {
      _httpClient = httpClient;
      _sharedLocalizer = sharedLocalizer;
      _httpContext = httpContext;
    }
    public async Task<UsuarioRespuestaViewModel> Login(LoginViewModel loginViewModel)
    {
      var contenido = ArmarContenido(loginViewModel);
      string cultura = ObtenerCultura(_httpContext);
     _httpClient.DefaultRequestHeaders.Add("culture", cultura);_httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
      var response = await _httpClient.PostAsync("GetLogin", contenido);
      if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
      {
        var usuarioRespuestaViewModel = new UsuarioRespuestaViewModel
        {
          ResponseResult = new ResponseResult()
        };
        usuarioRespuestaViewModel.ResponseResult.AgregarError(_sharedLocalizer["ContrasenaUsuarioIncorrecto"]);
        return usuarioRespuestaViewModel;
      }
      return await DeserializarObjetoResponse<UsuarioRespuestaViewModel>(response);
    }
  }
}
