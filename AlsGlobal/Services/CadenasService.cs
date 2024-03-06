using AlsGlobal.Extensions;
using AlsGlobal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AlsGlobal.Services
{
    public interface ICadenasService
    {
        Task<ResponseServiceViewModel<List<string>, List<string>, List<string>>> GetCadenas(FiltrosViewModel filtro, int page = 1, int rowPage = 20, int id_pais=1, int[] id_empresas = null);
        Task<ArchivoViewModel> GetDocumentosCadena(int[] id_documento, int id_plantilla);
        Task<JArray> GetCadenaPlantillas();
        Task<ArchivoViewModel> GetArchivoPlantilla(int id_documento);
        Task<JObject> EliminarArchivoPlantilla(int id_documento);
        Task<JObject> EnviarArchivoAExterno(string nombrePlantilla, IFormFile archivo);
        Task<JObject> EditarPlanilla(string nombrePlantilla, string id, IFormFile archivo);
        Task<JArray> GetEmpresas(string id_pais);
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
        public async Task<ResponseServiceViewModel<List<string>, List<string>, List<string>>> GetCadenas(FiltrosViewModel filtro, int page = 1, int rowPage = 20, int id_pais = 1, int[] id_empresas = null)
        {
            // Convertir filtros a un diccionario
            Dictionary<string, object> data = new Dictionary<string, object>();
            foreach (var prop in filtro.GetType().GetProperties())
            {
                data[prop.Name] = prop.GetValue(filtro);
            }

            // Añadir las propiedades adicionales al diccionario
            data["id_pais"] = id_pais;
            data["id_empresas"] = id_empresas;

            var contenido = ArmarContenido(data);
            string cultura = ObtenerCultura(_httpContext);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
            _httpClient.DefaultRequestHeaders.Add("culture", cultura); _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            var response = await _httpClient.PostAsync("GetDataCOC?page=" + page + "&rowPage=" + rowPage, contenido);

            var result = await DeserializarObjetoResponse<ResponseServiceViewModel<List<string>, List<string>, List<string>>>(response);

            return result;
        }

        public async Task<ArchivoViewModel> GetDocumentosCadena(int[] id_documento,int id_plantilla)
        {
            var contenido = ArmarContenido(
                new { 
                    id_muestras = id_documento,
                    id_plantilla
                });
            string cultura = ObtenerCultura(_httpContext);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
            _httpClient.DefaultRequestHeaders.Add("culture", cultura); _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            var response = await _httpClient.PostAsync("GetDocumentoCOC", contenido);
            var result = await DeserializarArchivoResponse(response);
            return new ArchivoViewModel(response.Content.Headers.ContentType.MediaType, response.Content.Headers.ContentDisposition?.FileName, result);//result;
        }

        public async Task<JArray> GetCadenaPlantillas()
        {
            string cultura = ObtenerCultura(_httpContext);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
            _httpClient.DefaultRequestHeaders.Add("culture", cultura); _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            var response = await _httpClient.PostAsync("GetCadenaPlantillas", null);
            //var result = await DeserializarArchivoResponse(response);
            var contentString = await response.Content.ReadAsStringAsync();

            var result = JArray.Parse(contentString);

            return result;
        }

        public async Task<ArchivoViewModel> GetArchivoPlantilla(int id_documento)
        {
            var contenido = ArmarContenido(new { id = id_documento });
            string cultura = ObtenerCultura(_httpContext);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
            _httpClient.DefaultRequestHeaders.Add("culture", cultura); _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            var response = await _httpClient.PostAsync("GetArchivoPlantilla", contenido);
            var result = await DeserializarArchivoResponse(response);
            return new ArchivoViewModel(response.Content.Headers.ContentType.MediaType, response.Content.Headers.ContentDisposition?.FileName, result);//result;
        }

        public async Task<JObject> EliminarArchivoPlantilla(int id_documento)
        {
            var contenido = ArmarContenido(new { id = id_documento });
            string cultura = ObtenerCultura(_httpContext);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
            _httpClient.DefaultRequestHeaders.Add("culture", cultura); _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            var response = await _httpClient.DeleteAsync("GetArchivoPlantilla/" + id_documento);
            response.EnsureSuccessStatusCode();

            var jsonContent = await response.Content.ReadAsStringAsync();
            var jsonObject = JObject.Parse(jsonContent);

            return jsonObject;
        }
        public async Task<JObject> EnviarArchivoAExterno(string nombrePlantilla, IFormFile archivo)
        {
            using var contenido = new MultipartFormDataContent();

            using var fileStream = archivo.OpenReadStream();
            using var memoryStream = new MemoryStream();
            await fileStream.CopyToAsync(memoryStream);
            byte[] archivoBytes = memoryStream.ToArray();

            var fileContent = new ByteArrayContent(archivoBytes);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            contenido.Add(fileContent, "plantilla", archivo.FileName);

            contenido.Add(new StringContent(nombrePlantilla), "nombre_plantilla");

            string cultura = ObtenerCultura(_httpContext);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
            _httpClient.DefaultRequestHeaders.Add("culture", cultura); _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            var response = await _httpClient.PostAsync("SetCadenaPlantilla", contenido);

            var jsonContent = await response.Content.ReadAsStringAsync();

            // Imprime el contenido de la respuesta
            Console.WriteLine(jsonContent);

            var jsonObject = JObject.Parse(jsonContent);

            return jsonObject;
        }

        public async Task<JObject> EditarPlanilla(string nombrePlantilla,string id, IFormFile archivo = null)
        {
            using var contenido = new MultipartFormDataContent();

            if(archivo != null)
            {
                using var fileStream = archivo.OpenReadStream();
                using var memoryStream = new MemoryStream();
                await fileStream.CopyToAsync(memoryStream);
                byte[] archivoBytes = memoryStream.ToArray();

                var fileContent = new ByteArrayContent(archivoBytes);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                contenido.Add(fileContent, "plantilla", archivo.FileName);
            }

            contenido.Add(new StringContent(nombrePlantilla), "nombre_plantilla");
            contenido.Add(new StringContent(id), "id");

            string cultura = ObtenerCultura(_httpContext);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
            _httpClient.DefaultRequestHeaders.Add("culture", cultura); _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            var response = await _httpClient.PostAsync("SetUpdatePlantilla", contenido);

            var jsonContent = await response.Content.ReadAsStringAsync();

            // Imprime el contenido de la respuesta
            Console.WriteLine(jsonContent);

            var jsonObject = JObject.Parse(jsonContent);

            return jsonObject;
        }
        public async Task<JArray> GetEmpresas(string id_pais)
        {
            var data = new
            {
                id_pais = id_pais
            };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            var contenido = new StringContent(json, Encoding.UTF8, "application/json");
            
            string cultura = ObtenerCultura(_httpContext);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
            _httpClient.DefaultRequestHeaders.Add("culture", cultura); _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            var response = await _httpClient.PostAsync("GetRegionEmpresas", contenido);

            var responseBody = await response.Content.ReadAsStringAsync();

            var result = JArray.Parse(responseBody);

            return result;
        }

    }
}
