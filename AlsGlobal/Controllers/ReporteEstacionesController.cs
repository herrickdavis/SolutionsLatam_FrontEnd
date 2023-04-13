using AlsGlobal.Models;
using AlsGlobal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlsGlobal.Controllers
{
  [Authorize]
  public class ReporteEstacionesController : Controller
  {
    private readonly IMatrizService _matrizService;
    private readonly IParametrosService _parametrosService;
    private readonly IEstacionesService _estacionesService;
    public ReporteEstacionesController(IMatrizService matrizService,
      IParametrosService parametrosService,
      IEstacionesService estacionesService)
    {
      _matrizService = matrizService;
      _parametrosService = parametrosService;
      _estacionesService = estacionesService;
    }
    public async Task<IActionResult> Index()
    {
      ViewBag.Matriz = await ObtenerMatriz();
      return View();
    }
    [HttpPost]
    public async Task<IActionResult> GetParametrosReporteEstaciones(ParametrosReporteEstacionRequestViewModel request)
    {
      var select = new List<SelectListItem>();
      var parametros = await _parametrosService.GetParametrosReporteEstaciones(request);
      if (parametros != null && parametros.Count() > 0)
      {
        select = parametros.Select(x => new SelectListItem(x.nombre_parametro, x.id)).ToList();
      }
      return Json(select);
    }
    [HttpPost]
    public async Task<IActionResult> GetEstacionesReporteEstaciones(EstacionesReporteEstacionesRequestViewModel request)
    {
      var select = new List<SelectListItem>();
      var parametros = await _estacionesService.GetEstacionesReporteEstaciones(request);
      if (parametros != null && parametros.Count() > 0)
      {
        select = parametros.Select(x => new SelectListItem(x.nombre_estacion, x.id)).ToList();
      }
      return Json(select);
    }
    [HttpPost]
    public async Task<IActionResult> GetReporteEstaciones(ReporteEstacionesRequestViewModel request)
    {
      var reporteEstacion = await _estacionesService.GetReporteEstaciones(request);
      return PartialView("_Detalle", reporteEstacion);
    }
    private async Task<IEnumerable<SelectListItem>> ObtenerMatriz()
    {
      var select = new List<SelectListItem>();
      var matrices = await _matrizService.GetMatrices();
      if (matrices != null && matrices.Count() > 0)
      {
        foreach(var matriz in matrices)
        {
          var selectGroup = new SelectListGroup();
          selectGroup.Name = matriz.nombre;

          select.AddRange(matriz.tipo_muestras.Select(x => new SelectListItem(x.tipo_muestra, x.id)
          {
            Group = selectGroup
          }).ToList());
        }
      }
      return select;
    }
  }
}
