using AlsGlobal.Extensions;
using AlsGlobal.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AlsGlobal.Services
{
  public interface IDataPreliminarService
  {
    Task<DataPreliminarViewModel> Obtener(FiltrosViewModel filtro, int page = 1, int rowPage = 20);
    Task<ArchivoViewModel> Reporte();
  }
  public class DataPreliminarService : Service, IDataPreliminarService
  {
    private readonly HttpClient _httpClient;
    private readonly IAspNetUser _aspNetUser;
    private readonly IHttpContextAccessor _httpContext;
    public DataPreliminarService(HttpClient httpClient,
                                IAspNetUser aspNetUser,
                                IHttpContextAccessor httpContext)
    {
      _httpClient = httpClient;
      _aspNetUser = aspNetUser;
      _httpContext = httpContext;
    }
    public async Task<DataPreliminarViewModel> Obtener(FiltrosViewModel filtro, int page = 1, int rowPage = 20)
    {
      var contenido = ArmarContenido(filtro);

      string cultura = ObtenerCultura(_httpContext);
      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
      _httpClient.DefaultRequestHeaders.Add("culture", cultura);
      _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
      var response = await _httpClient.PostAsync("GetDataCampo?page=" + page + "&rowPage=" + rowPage, contenido);
      if (!TratarError(response))
      {
        return new DataPreliminarViewModel
        {
          ResponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
        };
      }
      var result = await DeserializarObjetoResponse<DataPreliminarViewModel>(response);
      return result;

    }

    public async Task<ArchivoViewModel> Reporte()
    {
      var contenido = ArmarContenido();
      string cultura = ObtenerCultura(_httpContext);
      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
      _httpClient.DefaultRequestHeaders.Add("culture", cultura); 
      _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
      var response = await _httpClient.PostAsync("GetDataCampoExcel", contenido);
      if (!TratarError(response))
      {
        return null;
      }
      var result = await DeserializarArchivoResponse(response);
      return new ArchivoViewModel(response.Content.Headers.ContentType.MediaType.ToString(), response.Content.Headers.ContentDisposition.FileName, result);
    }
  }
}
