using AlsGlobal.Extensions;
using AlsGlobal.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using static AlsGlobal.Models.MuestraViewModel;

namespace AlsGlobal.Services
{
    public interface ITelemetriaService
    {
        Task<IEnumerable<TelemetriaInfoViewModel>> GetInfo();
        Task<IEnumerable<TelemetriaDataViewModel>> GetData(string[] nombre_estacion, int[] id_parametros);
        Task<IEnumerable<TelemetriaDataWindRoseViewModel>> GetDateWindRose(string[] nombre_estacion, int[] id_parametros);
        Task<IEnumerable<TelemetriaAllParametro>> GetAllParametro();
        Task<IEnumerable<TelemetriaAllGrupoParametro>> GetAllGrupo();
        Task<IEnumerable<TelemetriaAllStation>> GetAllStation();
        Task<string> SetLimite(string nombre_limite, TelemetriaSetLimite[] parametros, int id_limite);
        Task<IEnumerable<TelemetriaGetLimite>> GetAllLimite();
        Task<string> SetGrupoParametros(string nombre_grupo_parametro, TelemetriaSetGrupoParametros[] parametros, int id_grupo_parametro = 0);
        public class TelemetriaService : Service, ITelemetriaService
        {
            private readonly HttpClient _httpClient;
            private readonly IAspNetUser _aspNetUser;
            private readonly IHttpContextAccessor _httpContext;
            public TelemetriaService(HttpClient httpClient,
                                        IAspNetUser aspNetUser,
                                        IHttpContextAccessor httpContext)
            {
                _httpClient = httpClient;
                _aspNetUser = aspNetUser;
                _httpContext = httpContext;
            }

            public async Task<IEnumerable<TelemetriaInfoViewModel>> GetInfo()
            {
                var contenido = ArmarContenido(new { });
                string cultura = ObtenerCultura(_httpContext);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
                _httpClient.DefaultRequestHeaders.Add("culture", cultura); _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                var response = await _httpClient.PostAsync("GetInfoTelemetria", null);

                var result = await DeserializarObjetoResponse<IEnumerable<TelemetriaInfoViewModel>>(response);
                return result;
            }

            public async Task<IEnumerable<TelemetriaDataViewModel>> GetData(string[] nombre_estacion, int[] id_parametros)
            {
                var contenido = ArmarContenido(new {
                    nombre_estacion,
                    id_parametros
                });
                string cultura = ObtenerCultura(_httpContext);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
                _httpClient.DefaultRequestHeaders.Add("culture", cultura); _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                var response = await _httpClient.PostAsync("GetDataTelemetria", contenido);

                var result = await DeserializarObjetoResponse<IEnumerable<TelemetriaDataViewModel>>(response);
                return result;
            }

            public async Task<IEnumerable<TelemetriaAllParametro>> GetAllParametro()
            {
                string cultura = ObtenerCultura(_httpContext);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
                _httpClient.DefaultRequestHeaders.Add("culture", cultura); _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                var response = await _httpClient.PostAsync("getParameters", null);

                var result = await DeserializarObjetoResponse<IEnumerable<TelemetriaAllParametro>>(response);
                return result;
            }

            public async Task<IEnumerable<TelemetriaAllGrupoParametro>> GetAllGrupo()
            {
                string cultura = ObtenerCultura(_httpContext);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
                _httpClient.DefaultRequestHeaders.Add("culture", cultura); _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                var response = await _httpClient.PostAsync("getAllGroup", null);

                var result = await DeserializarObjetoResponse<IEnumerable<TelemetriaAllGrupoParametro>>(response);
                return result;
            }

            public async Task<IEnumerable<TelemetriaAllStation>> GetAllStation()
            {
                string cultura = ObtenerCultura(_httpContext);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
                _httpClient.DefaultRequestHeaders.Add("culture", cultura); _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                var response = await _httpClient.PostAsync("getAllStation", null);

                var result = await DeserializarObjetoResponse<IEnumerable<TelemetriaAllStation>>(response);
                return result;
            }
            public async Task<string> SetLimite(string nombre_limite, TelemetriaSetLimite[] parametros, int id_limite = 0)
            {
                var contenido = ArmarContenido(new
                {
                    id_limite,
                    nombre_limite,
                    parametros
                });
                string cultura = ObtenerCultura(_httpContext);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
                _httpClient.DefaultRequestHeaders.Add("culture", cultura); _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                var response = await _httpClient.PostAsync("setLimites", contenido);

                var responseString = await response.Content.ReadAsStringAsync();
                return responseString;
            }

            public async Task<IEnumerable<TelemetriaGetLimite>> GetAllLimite()
            {
                string cultura = ObtenerCultura(_httpContext);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
                _httpClient.DefaultRequestHeaders.Add("culture", cultura); _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                var response = await _httpClient.PostAsync("getAllLimites", null);

                var result = await DeserializarObjetoResponse<IEnumerable<TelemetriaGetLimite>>(response);
                return result;
            }

            public async Task<string> SetGrupoParametros(string nombre_grupo_parametro, TelemetriaSetGrupoParametros[] parametros, int id_grupo_parametro = 0)
            {
                var contenido = ArmarContenido(new
                {
                    id_grupo_parametro,
                    nombre_grupo_parametro,
                    parametros
                });
                string cultura = ObtenerCultura(_httpContext);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
                _httpClient.DefaultRequestHeaders.Add("culture", cultura); _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                var response = await _httpClient.PostAsync("setGrupoParametros", contenido);

                var responseString = await response.Content.ReadAsStringAsync();
                return responseString;
            }

            public async Task<IEnumerable<TelemetriaDataWindRoseViewModel>> GetDateWindRose(string[] nombre_estacion, int[] id_parametros)
            {
                var contenido = ArmarContenido(new
                {
                    nombre_estacion,
                    id_parametros
                });
                string cultura = ObtenerCultura(_httpContext);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
                _httpClient.DefaultRequestHeaders.Add("culture", cultura); _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                var response = await _httpClient.PostAsync("getDataWindRose", contenido);

                var result = await DeserializarObjetoResponse<IEnumerable<TelemetriaDataWindRoseViewModel>>(response);
                return result;
            }


        }
    }
}
