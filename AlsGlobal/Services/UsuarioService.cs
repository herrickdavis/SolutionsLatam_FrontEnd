using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AlsGlobal.Extensions;
using AlsGlobal.Models;
using Microsoft.AspNetCore.Http;

namespace AlsGlobal.Services{
    public interface IUsuarioService{
        Task<UsuarioResponseViewModel> Obtener(UsuarioRequestViewModel request, int page = 1, int rowPage = 20);
        Task<CrearUsuarioResponseViewModel> SetUsuario(CrearUsuarioRequestViewModel request);
        Task<CrearUsuarioResponseViewModel> SetUsuario(int id);
        Task<EditarUsuarioResponseViewModel> SetUsuario(EditarUsuarioRequestViewModel request);
        Task<EditarUsuarioRequestViewModel> Obtener(int id);
    }
    public class UsuarioService:Service, IUsuarioService{
        private readonly HttpClient _httpClient;
        private readonly IAspNetUser _aspNetUser;
        private readonly IHttpContextAccessor _httpContext;
        public UsuarioService(IHttpContextAccessor httpContext, HttpClient httpClient, IAspNetUser aspNetUser)
        {
            _httpContext = httpContext;
            _httpClient = httpClient;
            _aspNetUser = aspNetUser;
        }
        public async Task<UsuarioResponseViewModel> Obtener(UsuarioRequestViewModel request, int page = 1, int rowPage = 20)
        {
            var contenido = ArmarContenido(request);
            string cultura = ObtenerCultura(_httpContext);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
            _httpClient.DefaultRequestHeaders.Add("culture", cultura);
            var response = await _httpClient.PostAsync("GetUsuarios?page=" + page + "&rowPage=" + rowPage, contenido);
            var result = await DeserializarObjetoResponse<UsuarioResponseViewModel>(response);
            return result;
        }
        public async Task<EditarUsuarioRequestViewModel> Obtener(int id)
        {
            string cultura = ObtenerCultura(_httpContext);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
            _httpClient.DefaultRequestHeaders.Add("culture", cultura);
            var response = await _httpClient.GetAsync($"GetUsuarios/{id}");
            var result = await DeserializarObjetoResponse<EditarUsuarioRequestViewModel>(response);
            return result;
        }
        public async Task<CrearUsuarioResponseViewModel> SetUsuario(CrearUsuarioRequestViewModel request){
            var contenido = ArmarContenido(request);
            string cultura = ObtenerCultura(_httpContext);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
            _httpClient.DefaultRequestHeaders.Add("culture", cultura);
            var response = await _httpClient.PostAsync("SetUsuario", contenido);
            var result = await DeserializarObjetoResponse<CrearUsuarioResponseViewModel>(response);
            return result;
        }
        public async Task<CrearUsuarioResponseViewModel> SetUsuario(int id){
            string cultura = ObtenerCultura(_httpContext);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
            _httpClient.DefaultRequestHeaders.Add("culture", cultura);
            var response = await _httpClient.DeleteAsync($"SetUsuario/{id}");
            var result = await DeserializarObjetoResponse<CrearUsuarioResponseViewModel>(response);
            return result;
        }
        public async Task<EditarUsuarioResponseViewModel> SetUsuario(EditarUsuarioRequestViewModel request){
            var contenido = ArmarContenido(request);
            string cultura = ObtenerCultura(_httpContext);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aspNetUser.ObtenerToken());
            _httpClient.DefaultRequestHeaders.Add("culture", cultura);
            var response = await _httpClient.PutAsync($"SetUsuario/{request.id}", contenido);
            var result = await DeserializarObjetoResponse<EditarUsuarioResponseViewModel>(response);
            return result;
        }
    }
}