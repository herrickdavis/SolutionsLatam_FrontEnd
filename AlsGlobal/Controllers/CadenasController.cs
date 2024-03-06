using AlsGlobal.Extensions;
using AlsGlobal.Models;
using AlsGlobal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace AlsGlobal.Controllers
{
    [Authorize]
    public class CadenasController : MainController
    {
        private readonly ICadenasService _cadenasService;
        private readonly ILogger<DatosPreliminarController> _logger;
        private readonly IPerfilService _perfilService;
        private readonly IAspNetUser _aspNetUser;

        public CadenasController(ILogger<DatosPreliminarController> logger,
          ICadenasService cadenasService,
          IAspNetUser aspNetUser, IPerfilService perfilService)
        {
            _logger = logger;
            _aspNetUser = aspNetUser;
            _cadenasService = cadenasService;
            _perfilService = perfilService;
        }
        public async Task<IActionResult> Index()
        {
            var response = await _cadenasService.GetCadenas(new FiltrosViewModel(), 1);
            var response_plantillas = await _cadenasService.GetCadenaPlantillas();
            ViewBag.Plantillas = response_plantillas;
            AgregarBloque(response.pagina);            
            ViewBag.RowPage = 20;           
            ViewBag.ColumnasFilter = await _perfilService.GetColumnas(new ColumnasRequestViewModel(1));
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetPaginacionCadenas(FiltrosViewModel filtro, int page = 1, int rowPage = 20, int id_pais = 1, int[] id_empresas = null)
        {
            var response = await _cadenasService.GetCadenas(filtro, page, rowPage, id_pais, id_empresas);
            AgregarBloque(response.pagina);
            ViewBag.RowPage = rowPage;
            return PartialView("_Cadenas", response);
        }

        [HttpPost]
        public async Task<IActionResult> ObtenerCadenas(int[] id, int id_plantilla)
        {
            var response = await _cadenasService.GetDocumentosCadena(id, id_plantilla);
            return File(response.Stream, response.ContentType, response.Nombre);
        }

        [HttpPost]
        public async Task<IActionResult> ObtenerPlantillas()
        {
            var response = await _cadenasService.GetCadenaPlantillas();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ObtenerArchivoCadenas(int[] id, int id_plantilla)
        {
            var response = await _cadenasService.GetDocumentosCadena(id, id_plantilla);
            return File(response.Stream, response.ContentType, response.Nombre);
        }

        [HttpPost]
        public async Task<IActionResult> ObtenerArchivoPlantilla(int id)
        {
            var response = await _cadenasService.GetArchivoPlantilla(id);
            return File(response.Stream, response.ContentType, response.Nombre);
        }

        [HttpDelete]
        public async Task<IActionResult> EliminarArchivoPlantilla(int id)
        {
            var response = await _cadenasService.EliminarArchivoPlantilla(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CargarArchivoPlantilla(string nombrePlantilla, IFormFile archivo)
        {
            if (archivo == null || archivo.Length == 0)
                return BadRequest("Archivo no válido");

            var response = await _cadenasService.EnviarArchivoAExterno(nombrePlantilla, archivo);

            // Verifica la respuesta del servicio externo
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> EditarPlantilla(string nombrePlantilla, string id, IFormFile archivo = null)
        {
            var response = await _cadenasService.EditarPlanilla(nombrePlantilla, id, archivo);

            // Verifica la respuesta del servicio externo
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> ObtenerEmpresas(string id_pais)
        {
            var response = await _cadenasService.GetEmpresas(id_pais);

            // Usamos Count en lugar de Any()
            if (response == null || response.Count == 0)
            {
                return Json(new List<SelectListItem>());
            }

            // Debes castear cada ítem a JObject antes de acceder a sus propiedades
            var select = response
                            .Cast<JObject>()
                            .Select(x => new SelectListItem(x["nombre_empresa"].ToString(), x["id_empresa"].ToString()))
                            .ToList();

            return Json(select);
        }

    }
}
