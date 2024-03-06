using AlsGlobal.Models;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using AlsGlobal.Extensions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;
using System;
using System.Net.Http.Json;
using System.Text.Json;


namespace AlsGlobal.Services
{
    public interface IEddService
    {
        Task<JObject> SetPlanillaEdd(string nombre_reporte, string[] configuracion, int id = 0);
        Task<JObject> GetPlanilla(int id);
        Task<ArchivoViewModel> GetDocumento(FiltrosViewModel filtro, string[] id_muestras, string id, string numero_grupo, string year_grupo);
    }
    public class EddService : Service, IEddService
    {
        private readonly HttpClient _httpClient;
        private readonly IAspNetUser _aspNetUser;
        private readonly IHttpContextAccessor _httpContext;
        public EddService(HttpClient httpClient,
                                IAspNetUser aspNetUser,
                                IHttpContextAccessor httpContext)
        {
            _httpClient = httpClient;
            _aspNetUser = aspNetUser;
            _httpContext = httpContext;
        }
            
        public async Task<JObject> SetPlanillaEdd(string nombre_reporte, string[] configuracion, int id = 0)
        {
            var contenido = ArmarContenido(
                new {
                    id,
                    nombre_reporte,
                    configuracion
                });

            string cultura = ObtenerCultura(_httpContext);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
            _httpClient.DefaultRequestHeaders.Add("culture", cultura); _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            var response = await _httpClient.PostAsync("SetPlanillaEdd", contenido);
            var contentString = await response.Content.ReadAsStringAsync();
            var result = JObject.Parse(contentString);

            return result;
        }

        public async Task<JObject> GetPlanilla(int id)
        {
            string cultura = ObtenerCultura(_httpContext);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
            _httpClient.DefaultRequestHeaders.Add("culture", cultura); _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            var response = await _httpClient.GetAsync("GetPlanillaEdd/" + id.ToString());
            var contentString = await response.Content.ReadAsStringAsync();
            Console.WriteLine(contentString);
            var result = JObject.Parse(contentString);

            return result;
        }

        public async Task<ArchivoViewModel> GetDocumento(FiltrosViewModel filtro, string[] id_muestras, string id = "", string numero_grupo = "", string year_grupo = "")
        {
            //var contenido = ArmarContenido(filtro);
            var data = new
            {
                filtros = filtro,
                id,
                numero_grupo,
                year_grupo,
                id_muestras
            };

            var contenido = new StringContent(System.Text.Json.JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

            string cultura = ObtenerCultura(_httpContext);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
            _httpClient.DefaultRequestHeaders.Add("culture", cultura); _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            var response = await _httpClient.PostAsync("GetDocumentoEdd", contenido);
            var result = await DeserializarArchivoResponse(response);
            return new ArchivoViewModel(response.Content.Headers.ContentType.MediaType, response.Content.Headers.ContentDisposition?.FileName, result);//result;
        }
    }
}