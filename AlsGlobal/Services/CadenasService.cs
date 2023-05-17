using AlsGlobal.Extensions;
using AlsGlobal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AlsGlobal.Services
{
    public interface ICadenasService
    {
        Task<ResponseServiceViewModel<List<string>, List<string>, List<string>>> GetCadenas(FiltrosViewModel filtro, int page = 1, int rowPage = 20);
        Task<ArchivoViewModel> GetDocumentosCadena(int[] id_documento);
    }
    public class CadenasService : Service, ICadenasService
    {
        private readonly HttpClient _httpClient;
        private readonly IAspNetUser _aspNetUser;
        private readonly IHttpContextAccessor _httpContext;
        public CadenasService(HttpClient httpClient,
                                IAspNetUser aspNetUser,
                                IHttpContextAccessor httpContext)
        {
            _httpClient = httpClient;
            _aspNetUser = aspNetUser;
            _httpContext = httpContext;
        }
        public async Task<ResponseServiceViewModel<List<string>, List<string>, List<string>>> GetCadenas(FiltrosViewModel filtro, int page = 1, int rowPage = 20)
        {
            var contenido = ArmarContenido(filtro);
            string cultura = ObtenerCultura(_httpContext);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
            _httpClient.DefaultRequestHeaders.Add("culture", cultura); _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            var response = await _httpClient.PostAsync("GetDataCOC?page=" + page + "&rowPage=" + rowPage, contenido);
            //var response = await _httpClient.PostAsync("GetAllEstaciones", null);

            var result = await DeserializarObjetoResponse<ResponseServiceViewModel<List<string>, List<string>, List<string>>>(response);

            return result;
        }

        public async Task<ArchivoViewModel> GetDocumentosCadena(int[] id_documento)
        {
            var contenido = ArmarContenido(new { id_documento = id_documento });
            string cultura = ObtenerCultura(_httpContext);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
            _httpClient.DefaultRequestHeaders.Add("culture", cultura); _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            var response = await _httpClient.PostAsync("GetDocumentoCOC", contenido);
            var result = await DeserializarArchivoResponse(response);
            return new ArchivoViewModel(response.Content.Headers.ContentType.MediaType, response.Content.Headers.ContentDisposition?.FileName, result);//result;
        }
    }
}
