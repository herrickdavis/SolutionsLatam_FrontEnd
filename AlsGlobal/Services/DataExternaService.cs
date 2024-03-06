using AlsGlobal.Extensions;
using AlsGlobal.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Text;
using Newtonsoft.Json;
using System;
using System.IO;

namespace AlsGlobal.Services
{
    public interface IDataExternaService
    {
        Task<GetInfoModel> GetInfo();
        Task<List<GetDataExternaMuestra>> GetMuestras(int pagina);
        Task<bool> UploadFileDataExterna(IFormFile file);
        Task<bool> SetMuestras(List<GetDataExternaMuestra> muestras);
        Task<Stream> GetExcelDataExterna();
        Task<ColumnasUsuarioResponseViewModel> EliminarFila(int id);
        Task<ColumnasUsuarioResponseViewModel> agregarEstacion();
        Task<ColumnasUsuarioResponseViewModel> agregarProyecto();
        public class DataExternaService : Service, IDataExternaService
        {
            private readonly HttpClient _httpClient;
            private readonly IAspNetUser _aspNetUser;
            private readonly IHttpContextAccessor _httpContext;
            public DataExternaService(HttpClient httpClient,
                                    IAspNetUser aspNetUser,
                                    IHttpContextAccessor httpContext)
            {
                _httpClient = httpClient;
                _aspNetUser = aspNetUser;
                _httpContext = httpContext;
            }
            public async Task<GetInfoModel> GetInfo()
            {
                string cultura = ObtenerCultura(_httpContext);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
                _httpClient.DefaultRequestHeaders.Add("culture", cultura); _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                var response = await _httpClient.PostAsync("GetInfo", null);

                var responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<GetInfoModel>(responseBody);
                return result;
            }
            public async Task<List<GetDataExternaMuestra>> GetMuestras(int pagina)
            {
                var requestData = new { Pagina = pagina }; // Crear objeto con el número de página
                var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

                string cultura = ObtenerCultura(_httpContext);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
                _httpClient.DefaultRequestHeaders.Add("culture", cultura); _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                var response = await _httpClient.PostAsync("GetMuestrasDataExterna", content);

                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JsonConvert.SerializeObject(responseBody));
                var result = JsonConvert.DeserializeObject<List<GetDataExternaMuestra>>(responseBody);
                return result;
            }

            public async Task<bool> SetMuestras(List<GetDataExternaMuestra> muestras)
            {
                //Console.WriteLine(JsonConvert.SerializeObject(muestras));
                var content = new StringContent(JsonConvert.SerializeObject(muestras), Encoding.UTF8, "application/json");

                string cultura = ObtenerCultura(_httpContext);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
                _httpClient.DefaultRequestHeaders.Add("culture", cultura); _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                var response = await _httpClient.PostAsync("SetMuestrasDataExterna", content);

                return response.IsSuccessStatusCode;
            }

            public async Task<bool> UploadFileDataExterna(IFormFile file)
            {
                string cultura = ObtenerCultura(_httpContext);
                using (var content = new MultipartFormDataContent())
                {
                    using (var fileStream = file.OpenReadStream())
                    {
                        content.Add(new StreamContent(fileStream), "excel", file.FileName);
                        _httpClient.Timeout = TimeSpan.FromMinutes(10);
                        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
                        _httpClient.DefaultRequestHeaders.Add("culture", cultura); _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                        var response = await _httpClient.PostAsync("SetDataExternaArchivo", content);
                        return response.IsSuccessStatusCode;
                    }
                }
            }

            public async Task<Stream> GetExcelDataExterna()
            {
                string cultura = ObtenerCultura(_httpContext);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
                _httpClient.DefaultRequestHeaders.Add("culture", cultura);
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/octet-stream"));

                var response = await _httpClient.PostAsync("GetExcelDataExternaPorValidar", null);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Error al obtener el archivo Excel.");
                }

                return await response.Content.ReadAsStreamAsync();
            }

            public async Task<ColumnasUsuarioResponseViewModel> EliminarFila(int id)
            {
                Console.WriteLine("ID: " + id);
                string cultura = ObtenerCultura(_httpContext);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
                _httpClient.DefaultRequestHeaders.Add("culture", cultura); _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                var response = await _httpClient.DeleteAsync("DataExternaEliminarMuestra/" + id);

                var responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ColumnasUsuarioResponseViewModel>(responseBody);
                return result;
            }

            public async Task<ColumnasUsuarioResponseViewModel> agregarEstacion()
            {
                //var content = new StringContent(JsonConvert.SerializeObject(estacion), Encoding.UTF8, "application/json");
                string cultura = ObtenerCultura(_httpContext);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
                _httpClient.DefaultRequestHeaders.Add("culture", cultura); _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                var response = await _httpClient.PostAsync("DataExternaCrearEstacion", null);

                var responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ColumnasUsuarioResponseViewModel>(responseBody);
                return result;
            }

            public async Task<ColumnasUsuarioResponseViewModel> agregarProyecto()
            {
                //var content = new StringContent(JsonConvert.SerializeObject(proyecto), Encoding.UTF8, "application/json");
                string cultura = ObtenerCultura(_httpContext);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
                _httpClient.DefaultRequestHeaders.Add("culture", cultura); _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                var response = await _httpClient.PostAsync("DataExternaCrearProyecto", null);

                var responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ColumnasUsuarioResponseViewModel>(responseBody);
                return result;
            }
        }
    }
}