using AlsGlobal.Extensions;
using AlsGlobal.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;

namespace AlsGlobal.Services
{
    public interface INotificacionService
    {
        Task<IEnumerable<NotificacionViewModel>> GetNotificaciones();
        public class NotificacionService : Service, INotificacionService
        {
            private readonly HttpClient _httpClient;
            private readonly IAspNetUser _aspNetUser;
            private readonly IHttpContextAccessor _httpContext;
            public NotificacionService(HttpClient httpClient,
                                        IAspNetUser aspNetUser,
                                        IHttpContextAccessor httpContext)
            {
                _httpClient = httpClient;
                _aspNetUser = aspNetUser;
                _httpContext = httpContext;
            }

            public async Task<IEnumerable<NotificacionViewModel>> GetNotificaciones()
            {
                var contenido = ArmarContenido(new { });
                string cultura = ObtenerCultura(_httpContext);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
                _httpClient.DefaultRequestHeaders.Add("culture", cultura); _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                var response = await _httpClient.PostAsync("Notificaciones", null);

                var result = await DeserializarObjetoResponse<IEnumerable<NotificacionViewModel>>(response);
                return result;
            }
        }
    }
}
