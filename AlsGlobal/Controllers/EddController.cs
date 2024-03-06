using AlsGlobal.Extensions;
using AlsGlobal.Models;
using AlsGlobal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;

namespace AlsGlobal.Controllers
{
    [Authorize]
    public class EddController : MainController
    {
        private readonly IEddService _eddService;
        private readonly ILogger<DatosPreliminarController> _logger;
        private readonly IPerfilService _perfilService;
        private readonly IAspNetUser _aspNetUser;

        public EddController(ILogger<DatosPreliminarController> logger,
          IEddService EddService,
          IAspNetUser aspNetUser, IInformesService informesService,
          IPerfilService perfilService)
        {
            _logger = logger;
            _aspNetUser = aspNetUser;
            _eddService = EddService;
            _perfilService = perfilService;
        }
        public async Task<IActionResult> Index(int? id)
        {
            //var response = await _cadenasService.GetCadenas(new FiltrosViewModel(), 1);
            //var response_plantillas = await _cadenasService.GetCadenaPlantillas();
            //ViewBag.Plantillas = response_plantillas;
            //AgregarBloque(response.pagina);
            //ViewBag.RowPage = 20;
            //ViewBag.ColumnasFilter = await _perfilService.GetColumnas(new ColumnasRequestViewModel(1));
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> EnviarPlanilla(string nombre_reporte, string[] configuracion, int id = 0)
        {
            var response = await _eddService.SetPlanillaEdd(nombre_reporte, configuracion, id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetPlanilla(int id)
        {
            var response = await _eddService.GetPlanilla(id);
            return Content(response.ToString(Formatting.None), "application/json");
        }

        [HttpPost]
        public async Task<IActionResult> GetDocumento(FiltrosViewModel filtro, string[] id_muestras, string id = "", string numero_grupo = "", string year_grupo = "")
        {
            var response = await _eddService.GetDocumento(filtro, id_muestras, id, numero_grupo, year_grupo);
            return File(response.Stream, response.ContentType, response.Nombre);
        }
    }
}
