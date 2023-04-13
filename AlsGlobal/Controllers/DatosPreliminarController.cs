using AlsGlobal.Extensions;
using AlsGlobal.Models;
using AlsGlobal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AlsGlobal.Controllers
{

  [Authorize]
  public class DatosPreliminarController : MainController
  {
    private readonly IDataPreliminarService _alsGlobalService;
    private readonly ILogger<DatosPreliminarController> _logger;
    private readonly IAspNetUser _aspNetUser;
    public DatosPreliminarController(ILogger<DatosPreliminarController> logger,
      IDataPreliminarService alsGlobalService,
      IAspNetUser aspNetUser)
    {
      _logger = logger;
      _aspNetUser = aspNetUser;
      _alsGlobalService = alsGlobalService;
    }
    public async Task<IActionResult> Index()
    {
      var response = await _alsGlobalService.Obtener(new FiltrosViewModel(), 1);
      AgregarBloque(response.pagina);
      ViewBag.RowPage = 20;
      return View(response);
    }
    public async Task<IActionResult> ObtenerReporte()
    {
      var response = await _alsGlobalService.Reporte();
      return File(response.Stream, response.ContentType, $"Reporte_{DateTime.Now}.xlsx");
    }
    [HttpPost]
    public async Task<IActionResult> ObtenerDataPreliminar(FiltrosViewModel filtro, [FromQuery] int page = 1, [FromQuery] int rowPage = 20)
    {
      var response = await _alsGlobalService.Obtener(filtro, page, rowPage);
      AgregarBloque(response.pagina);
      ViewBag.RowPage = rowPage;
      return PartialView("_DataPreliminar", response);
    }

    public IActionResult Privacy()
    {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { Title = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
