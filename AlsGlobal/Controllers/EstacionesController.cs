using AlsGlobal.Extensions;
using AlsGlobal.Models;
using AlsGlobal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace AlsGlobal.Controllers
{
    [Authorize]
    public class EstacionesController : MainController
    {
        private readonly IAllEstacionesService _allEstacionesService;
        private readonly IProyectoService _proyectosService;
        private readonly ILogger<DatosPreliminarController> _logger;
        private readonly IPerfilService _perfilService;
        private readonly IAspNetUser _aspNetUser;

        public EstacionesController(ILogger<DatosPreliminarController> logger,
          IAllEstacionesService allEstacionesService,
          IProyectoService ProyectoService,
          IAspNetUser aspNetUser, IPerfilService perfilService)
        {
            _logger = logger;
            _aspNetUser = aspNetUser;
            _allEstacionesService = allEstacionesService;
            _proyectosService = ProyectoService;
            _perfilService = perfilService;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _allEstacionesService.GetAllEstaciones(new FiltrosViewModel(), 1);
            var responseProyecto = await _proyectosService.GetAllProyectos(new FiltrosViewModel(), 1);
            AgregarBloque(response.pagina);
            AgregarBloque(responseProyecto.pagina);
            ViewBag.RowPage = 20;
            ViewBag.Proyecto = responseProyecto;
            ViewBag.ColumnasFilter = await _perfilService.GetColumnas(new ColumnasRequestViewModel(1));
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> ObtenerEstaciones(FiltrosViewModel filtro, [FromQuery] int page = 1, [FromQuery] int rowPage = 20)
        {
            var response = await _allEstacionesService.GetAllEstaciones(filtro, page, rowPage);            
            AgregarBloque(response.pagina);            
            ViewBag.RowPage = rowPage;            
            return PartialView("_Estaciones", response);
        }

        [HttpPost]
        public async Task<IActionResult> ObtenerProyectos(FiltrosViewModel filtro, [FromQuery] int page = 1, [FromQuery] int rowPage = 20)
        {
            var response = await _proyectosService.GetAllProyectos(filtro, page, rowPage);
            AgregarBloque(response.pagina);
            ViewBag.RowPage = rowPage;
            return PartialView("_Proyectos", response);
        }

        [HttpPost]
        public async Task<IActionResult> AsignarAlias(string[] id, string alias)
        {
            var response = await _allEstacionesService.SetAlias(new AsignarAliasRequestViewModel
            {
                id = id,
                alias = alias
            });

            
            return Json(response);
        }

        [HttpPost]
        public async Task<IActionResult> AsignarAliasProyectos(string[] id, string alias)
        {
            var response = await _proyectosService.SetAlias(new AsignarAliasRequestViewModel
            {
                id = id,
                alias = alias
            });


            return Json(response);
        }
    }
}
