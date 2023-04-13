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
  public class FueraDeLimiteController : MainController
  {
    private readonly IMuestrasService _muestrasService;
    private readonly ILogger<DatosPreliminarController> _logger;
    private readonly IAspNetUser _aspNetUser;
    private readonly IStringLocalizer<SharedResources> _stringLocalizer;
    private readonly IReporteHistoricoService _reporteHistoricoService;
    private readonly IFueraLimiteService _fueraLimiteService;
    public FueraDeLimiteController(ILogger<DatosPreliminarController> logger,
      IMuestrasService muestrasService,
      IReporteHistoricoService reporteHistoricoService,
      IFueraLimiteService fueraLimiteService,
      IAspNetUser aspNetUser,
      IStringLocalizer<SharedResources> stringLocalizer)
    {
      _logger = logger;
      _aspNetUser = aspNetUser;
      _muestrasService = muestrasService;
      _stringLocalizer = stringLocalizer;
      _reporteHistoricoService = reporteHistoricoService;
      _fueraLimiteService = fueraLimiteService;
    }
    public async Task<IActionResult> Index()
    {
      ViewBag.TipoMuestra = await ObtenerMuestras(new DateTime(2019,12,31), DateTime.Now); 
      ViewBag.Estaciones = new List<SelectListItem>()
      {
        new SelectListItem(_stringLocalizer["ValorDefectoSelect"], null)
      };
      return View(new FueraDeLimiteRequestViewModel
      {
        fecha_inicio = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd"),
        fecha_fin = DateTime.Now.ToString("yyyy-MM-dd")
      }) ;
    }
    public async Task<IActionResult> GetTipoMuestras(DateTime fecha_inicio, DateTime fecha_fin)
    {
      var select = await ObtenerMuestras(fecha_inicio, fecha_fin);
      return Json(select);
    }
    public async Task<IActionResult> ObtenerEstaciones(DateTime fecha_inicio, DateTime fecha_fin, string id_tipo_muestra)
    {
      var select = new List<SelectListItem>();
      var estaciones = await _reporteHistoricoService.GetEstaciones(new EstacionesRequestViewModel(fecha_inicio, fecha_fin, id_tipo_muestra));
      if (estaciones != null && estaciones.Count() > 0)
      {
        select = estaciones.Select(x => new SelectListItem(x.nombre_estacion, x.id)).ToList();
      }
      return Json(select);
    }
    [HttpPost]
    public async Task<IActionResult> GetReporteFueraLimiteExcel(FueraDeLimiteRequestViewModel request)
    {
      var response = await _fueraLimiteService.GetReporteFueraLimiteExcel(request);
      return File(response.Stream, response.ContentType, response.Nombre);
    }
    [HttpPost]
    public async Task<IActionResult> GetReporteFueraLimites(FueraDeLimiteRequestViewModel request)
    {
      var response = await _fueraLimiteService.GetReporteFueraLimites(request);

      return PartialView("_Detalle", response);
    }

    private async Task<List<SelectListItem>> ObtenerMuestras(DateTime fecha_inicio, DateTime fecha_fin)
    {
      var select = new List<SelectListItem>();
      var response = await _muestrasService.GetTipoMuestras(new TipoMuestraRequestViewModel(fecha_inicio, fecha_fin, true));

      if (response != null && response.Count() > 0)
      {
        select = response.Select(x => new SelectListItem(x.nombre_tipo_muestra, x.id)).ToList();
      }
      return select;
    }
  }
}
