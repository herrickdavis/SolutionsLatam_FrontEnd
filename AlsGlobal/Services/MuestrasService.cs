using AlsGlobal.Extensions;
using AlsGlobal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AlsGlobal.Services
{
  public interface IMuestrasService
  {
    Task<ResponseServiceViewModel<List<string>, List<string>, List<string>>> Obtener(FiltrosViewModel filtro, int page = 1, int rowPage = 20);
    Task<MuestraViewModel> Obtener(MuestraRequestViewModel request);
    Task<IEnumerable<TipoMuestraViewModel>> GetTipoMuestras(TipoMuestraRequestViewModel request);
    Task<IEnumerable<DocumentoMuestraViewModel>> GetDocumentosMuestra(DocumentoMuestraRequestViewModel request);
    Task<ArchivoViewModel> GetDocumentoMuestra(DocumentoMuestraRequestViewModel request);
    Task<ArchivoViewModel> GetZipMuestra(DocumentoMuestraRequestViewModel request);
    Task<JArray> ObtenerEdd();
  }
  public class MuestrasService : Service, IMuestrasService
  {
    private readonly HttpClient _httpClient;
    private readonly IAspNetUser _aspNetUser;
    private readonly IHttpContextAccessor _httpContext;
    public MuestrasService(HttpClient httpClient,
                                IAspNetUser aspNetUser,
                                IHttpContextAccessor httpContext)
    {
      _httpClient = httpClient;
      _aspNetUser = aspNetUser;
      _httpContext = httpContext;
    }
    public async Task<ResponseServiceViewModel<List<string>, List<string>, List<string>>> Obtener(FiltrosViewModel filtro, int page = 1, int rowPage = 20)
    {
      var contenido = ArmarContenido(filtro);
      string cultura = ObtenerCultura(_httpContext);
      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
      _httpClient.DefaultRequestHeaders.Add("culture", cultura); _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
      var response = await _httpClient.PostAsync("GetMuestras?page=" + page + "&rowPage=" + rowPage, contenido);

      if (!TratarError(response))
      {
        return new ResponseServiceViewModel<List<string>, List<string>, List<string>>
        {
          ResponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
        };
      }
      var result = await DeserializarObjetoResponse<ResponseServiceViewModel<List<string>, List<string>, List<string>>>(response);
      return result;
    }
    public async Task<MuestraViewModel> Obtener(MuestraRequestViewModel request)
    {
      var contenido = ArmarContenido(request);
      string cultura = ObtenerCultura(_httpContext);
      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
      _httpClient.DefaultRequestHeaders.Add("culture", cultura);
      _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
      var response = await _httpClient.PostAsync("GetMuestra", contenido);
      if (!TratarError(response))
      {
        return new MuestraViewModel
        {
          ResponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
        };
      }
      var result = await DeserializarObjetoResponse<MuestraViewModel>(response);
      return result;
    }
    public async Task<IEnumerable<TipoMuestraViewModel>> GetTipoMuestras(TipoMuestraRequestViewModel request)
    {
      var contenido = ArmarContenido(request);
      string cultura = ObtenerCultura(_httpContext);
      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
      _httpClient.DefaultRequestHeaders.Add("culture", cultura); 
      _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
      var response = await _httpClient.PostAsync("GetTipoMuestras", contenido);


      var result = await DeserializarObjetoResponse<IEnumerable<TipoMuestraViewModel>>(response);
      return result;
    }

    public async Task<IEnumerable<DocumentoMuestraViewModel>> GetDocumentosMuestra(DocumentoMuestraRequestViewModel request)
    {
      var contenido = ArmarContenido(request);
      string cultura = ObtenerCultura(_httpContext);
      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
      _httpClient.DefaultRequestHeaders.Add("culture", cultura); 
      _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
      var response = await _httpClient.PostAsync("GetDocumentosMuestra", contenido);
      var result = await DeserializarObjetoResponse<IEnumerable<DocumentoMuestraViewModel>>(response);
      return result;
    }
    public async Task<ArchivoViewModel> GetDocumentoMuestra(DocumentoMuestraRequestViewModel request)
    {
      var contenido = ArmarContenido(request);
      string cultura = ObtenerCultura(_httpContext);
      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
      _httpClient.DefaultRequestHeaders.Add("culture", cultura); 
      _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
      var response = await _httpClient.PostAsync("GetDocumentoMuestra", contenido);
      var result = await DeserializarArchivoResponse(response);

      return new ArchivoViewModel(response.Content.Headers.ContentType.MediaType, response.Content.Headers.ContentDisposition?.FileName, result);//result;
    }
    public async Task<ArchivoViewModel> GetZipMuestra(DocumentoMuestraRequestViewModel request)
    {
      var contenido = ArmarContenido(request);
      string cultura = ObtenerCultura(_httpContext);
      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
      _httpClient.DefaultRequestHeaders.Add("culture", cultura); 
      _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
      var response = await _httpClient.PostAsync("GetZipMuestra", contenido);
      var result = await DeserializarArchivoResponse(response);
      return new ArchivoViewModel(response.Content.Headers.ContentType.MediaType, response.Content.Headers.ContentDisposition?.FileName, result);//result;
    }

    public async Task<JArray> ObtenerEdd()
    {
            string cultura = ObtenerCultura(_httpContext);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
            _httpClient.DefaultRequestHeaders.Add("culture", cultura); _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            var response = await _httpClient.PostAsync("GetPlanillaEdd", null);
            var contentString = await response.Content.ReadAsStringAsync();
            var result = JArray.Parse(contentString);

            return result;
        }
    }
}
