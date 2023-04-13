using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlsGlobal.Extensions;
using AlsGlobal.Models;
using AlsGlobal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlsGlobal.Controllers{
    [Authorize(Roles = "1,2,3")]
    public class UsuarioController: MainController{
        private readonly IEmpresaService _empresaService;
        private readonly IUsuarioService _usuarioService;
        private readonly IRolService _rolService;
        private readonly IIdiomasService _idiomasService;
        private readonly IAspNetUser _user;
        public UsuarioController(IAspNetUser user, IUsuarioService usuarioService, IEmpresaService empresaService, IRolService rolService, IIdiomasService idiomasService)
        {
            _user = user;
            _usuarioService = usuarioService;
            _empresaService = empresaService;
            _rolService = rolService;
            _idiomasService = idiomasService;
        }
        public async Task<IActionResult> Index(){
            var response = await _usuarioService.Obtener(new UsuarioRequestViewModel());
            AgregarBloque(response);
            ViewBag.RowPage = 20;
            return View(response);
        }
        public async Task<IActionResult> Crear(){
            ViewBag.Empresas = await ArmarSelectEmpresas();
            ViewBag.Roles = await ArmarSelectRol();
            ViewBag.Idiomas = await ArmarSelectIdiomas();
            return View(new CrearUsuarioRequestViewModel{
                id_rol = 4,
                idioma = "es",
                data_campo = "N"
            });
        }
        public async Task<IActionResult> Editar(int id){
            var response = await _usuarioService.Obtener(id);
            ViewBag.Empresas = await ArmarSelectEmpresas(response.id_empresas);
            ViewBag.Roles = await ArmarSelectRol();
            ViewBag.Idiomas = await ArmarSelectIdiomas();
            return View(response);
        }
        public async Task<IActionResult> Filtrar(UsuarioRequestViewModel request, [FromQuery] int page = 1, [FromQuery] int rowPage = 20){
            var response = await _usuarioService.Obtener(request, page, rowPage);
            AgregarBloque(response);
            ViewBag.RowPage = rowPage;
            return PartialView("_Usuario",response);
        }
        public IActionResult GenerarContrasena(){
            string contrasena = "";
            var random = new Random();
            int[] ignoreChar = new int[]{58,59,60,61,62,63,64,91,92,93,94,95,96};
            while(contrasena.Length <= 10){
                var num = random.Next(48,122);
                if(ignoreChar.Contains(num)) continue;
                contrasena += (char)num;
            }
            return Json(contrasena);
        }
        [HttpPost]
        public async Task<IActionResult> Crear(CrearUsuarioRequestViewModel request){
            if(!ModelState.IsValid){
                return Json(new CrearUsuarioResponseViewModel{error="true", mensaje = string.Join(" | ", ModelState.SelectMany(x=>x.Value.Errors).Select(x => x.ErrorMessage).ToArray())});
            }
            var response = await _usuarioService.SetUsuario(request);
             return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> Editar(EditarUsuarioRequestViewModel request){
            if(!ModelState.IsValid){
                return Json(new EditarUsuarioResponseViewModel{error="true", mensaje = string.Join(" | ", ModelState.SelectMany(x=>x.Value.Errors).Select(x => x.ErrorMessage).ToArray())});
            }
            var response = await _usuarioService.SetUsuario(request);
            
            
             return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> ActivarDesactivarUsuario(int id){
            var response = await _usuarioService.SetUsuario(id);
            return Json(response);
        }
        private async Task<IEnumerable<SelectListItem>> ArmarSelectEmpresas(IEnumerable<int> empresas = null)
        {
            var select = new List<SelectListItem>();
            var response = await _empresaService.Obtener();

            if (response != null && response.Count() > 0)
            {
                if(empresas == null)
                    select = response.Select(x => new SelectListItem(x.nombre_empresa, x.id.ToString())).ToList();
                else
                    select = response.Select(x => new SelectListItem(x.nombre_empresa, x.id.ToString(), empresas.Any(y=>y == x.id) )).ToList();
            }
            return select;
        }
        private async Task<IEnumerable<SelectListItem>> ArmarSelectRol()
        {
            var select = new List<SelectListItem>();
            var response = await _rolService.Obtener();

            if (response != null && response.Count() > 0)
            {
                select = response.Select(x => new SelectListItem(x.Description, x.Id.ToString())).ToList();
            }
            return select;
        }
         private async Task<IEnumerable<SelectListItem>> ArmarSelectIdiomas()
        {
            var select = new List<SelectListItem>();
            var response = await _idiomasService.Obtener();

            if (response != null && response.Count() > 0)
            {
                select = response.Select(x => new SelectListItem(x.Description, x.Value)).ToList();
            }
            return select;
        }
    }
}