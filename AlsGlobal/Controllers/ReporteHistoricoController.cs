using AlsGlobal.Extensions;
using AlsGlobal.Models;
using AlsGlobal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlsGlobal.Controllers
{
  [Authorize]
  public class ReporteHistoricoController : MainController
  {
    private readonly IMuestrasService _muestrasService;
    private readonly IReporteHistoricoService _reporteHistoricoService;
    private readonly ILimitesService _limitesService;
    private readonly IProyectoService _proyectoService;
    private readonly IAspNetUser _aspNetUser;
    private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        public ReporteHistoricoController(
          IReporteHistoricoService reporteHistoricoService,
          IMuestrasService muestrasService,
          ILimitesService limitesService,
          IAspNetUser aspNetUser,
          IStringLocalizer<SharedResources> stringLocalizer, IProyectoService proyectoService)
        {
            _aspNetUser = aspNetUser;
            _reporteHistoricoService = reporteHistoricoService;
            _limitesService = limitesService;
            _muestrasService = muestrasService;
            _stringLocalizer = stringLocalizer;
            _proyectoService = proyectoService;
        }
    public async Task<IActionResult> Detalle(int id)
    {
      var reporteHistoricoRequestViewModel = new ReporteHistoricoRequestViewModel
      {
        id_matriz = id
      };
      ViewBag.TipoMuestras = await ArmarSelectTipoMuestras(id);
      return View(reporteHistoricoRequestViewModel);
    }
    public async Task<IActionResult> GetTipoMuestras(DateTime fecha_inicio, DateTime fecha_fin)
    {
      var select = await ArmarSelectTipoMuestras(0);
      return Json(select);
    }
    [HttpPost]
    public async Task<IActionResult> GetDataHistoricaExcel(ReporteHistoricoRequestViewModel request)
    {
      var response = await _reporteHistoricoService.GetDataHistoricaExcel(request);
      return File(response.Stream, response.ContentType, response.Nombre);
    }
    [HttpPost]
    public async Task<IActionResult> ObtenerEstacionesProyectosYLimites(EstacionesRequestViewModel request)
    { 
      var estaciones = await ArmarSelectEstaciones(request);
      var limite = await ArmarSelectLimites(request.id_tipo_muestra);
      var proyectos = await ArmarSelectProyectos(request.id_tipo_muestra);
      return Json(new { estacion = estaciones, proyectos = proyectos, Limite = limite });
    }
    [HttpPost]
    public async Task<IActionResult> ObtenerEstaciones(EstacionesRequestViewModel request)
    { 
      var estaciones = await ArmarSelectEstaciones(request);
      return Json(estaciones);
    }
    [HttpPost]
    public async Task<IActionResult> ObtenerParametros(ParametrosRequestViewModel request)
    {
      var parametros = await _reporteHistoricoService.GetParametros(request);
      return Json(parametros);
    }

    [HttpPost]
    public async Task<IActionResult> ObtenerGraficos(ReporteHistoricoRequestViewModel request)
    {
      var parametros = await _reporteHistoricoService.GetGrafico(request);
      return Json(parametros);
    }

    private async Task<IEnumerable<SelectListItem>> ArmarSelectTipoMuestras(int id_matriz)
    {
      var select = new List<SelectListItem>();
      var response = await _muestrasService.GetTipoMuestras(new TipoMuestraRequestViewModel(DateTime.Now, DateTime.Now, id_matriz, false));

      if (response != null && response.Count() > 0)
      {
        select = response.Select(x => new SelectListItem(x.nombre_tipo_muestra, x.id)).ToList();
      }
      return select;
    }
    private async Task<IEnumerable<SelectListItem>> ArmarSelectLimites(string id_tipo_muestra){
      var response = await _limitesService.GetLimites(new LimitesRequestViewModel(id_tipo_muestra));
      var select = new List<SelectListItem>();
      if (response != null && response.Count() > 0)
      {
        select = response.Select(x => new SelectListItem(x.nombre_limite, x.id.ToString())).ToList();
      }
      return select;
    }
    private async Task<IEnumerable<SelectListItem>> ArmarSelectEstaciones(EstacionesRequestViewModel request){
      var response = await _reporteHistoricoService.GetEstaciones(request);
      var select = new List<SelectListItem>();
      if (response != null && response.Count() > 0)
      {
        select = response.Select(x => new SelectListItem(x.nombre_estacion, x.id.ToString())).ToList();
      }
      return select;
    }
      private async Task<IEnumerable<SelectListItem>> ArmarSelectProyectos(string id_tipo_muestra){
      var response = await _proyectoService.GetProyectos(new ProyectoRequestViewModel{ id_tipo_muestra = id_tipo_muestra });
      var select = new List<SelectListItem>();
      if (response != null && response.Count() > 0)
      {
        select = response.Select(x => new SelectListItem(x.nombre_proyecto, x.id.ToString())).ToList();
      }
      return select;
    }
  }
}
