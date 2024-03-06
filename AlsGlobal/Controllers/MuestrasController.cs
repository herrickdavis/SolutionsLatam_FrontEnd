using AlsGlobal.Extensions;
using AlsGlobal.Models;
using AlsGlobal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace AlsGlobal.Controllers
{
  [Authorize]
  public class MuestrasController : MainController
  {
    private readonly IMuestrasService _muestrasService;
    private readonly IInformesService _informesService;
    private readonly ILogger<DatosPreliminarController> _logger;
    private readonly IPerfilService _perfilService;
    private readonly IAspNetUser _aspNetUser;

        public MuestrasController(ILogger<DatosPreliminarController> logger,
          IMuestrasService muestrasService,
          IAspNetUser aspNetUser, IInformesService informesService, 
          IPerfilService perfilService)
        {
            _logger = logger;
            _aspNetUser = aspNetUser;
            _muestrasService = muestrasService;
            _informesService = informesService;
            _perfilService = perfilService;
        }
    public async Task<IActionResult> Index()
    {
      //var response = await _muestrasService.Obtener(new FiltrosViewModel(), 1);
      var response = new ResponseServiceViewModel<List<string>, List<string>, List<string>>();
            response.cabecera = new List<string> { "---", "---", "---", "---", "---", "---", "---", "---", "---", "---" };
            response.pagina = new Pagina<List<string>>();
            response.pagina.data = new List<Data<List<string>>>();
    //var responseInforme = await _informesService.Obtener(new FiltrosViewModel(), 1);
    //AgregarBloque(response.pagina);
    //AgregarBloque(responseInforme.pagina);
    ViewBag.Detalle = new MuestraViewModel();

      var responseInforme = new ResponseServiceViewModel<List<string>, List<string>, List<string>>();
      responseInforme.cabecera = new List<string> { "id", "Numero", "Titulo Certificado" };
      responseInforme.pagina = new Pagina<List<string>>();
      responseInforme.pagina.data = new List<Data<List<string>>>();
      ViewBag.Informe = responseInforme;
      ViewBag.RowPage = 20;
      ViewBag.ColumnasFilter = await _perfilService.GetColumnas(new ColumnasRequestViewModel(1));
      return View(response);
    }
    public async Task<IActionResult> Detalle(int id)
    {
      MuestraRequestViewModel request = new MuestraRequestViewModel
      {
        id_muestra = id
      };
      var response = await _muestrasService.Obtener(request);
      return PartialView("_Detalle", response);
    }
    [HttpGet]
    public async Task<IActionResult> ObtenerParametros(MuestraRequestViewModel model)
    {
      var response = await _muestrasService.Obtener(model);
      ViewBag.mostrar_limite = response.mostrar_limite;
      return PartialView("_Parametros", response.parametros);
    }
    [HttpGet]
    public async Task<IActionResult> GetDocumentoMuestra(string id)
    {
      var response = await _muestrasService.GetDocumentoMuestra(new DocumentoMuestraRequestViewModel{
        id_documento = id
      });

      return File(response.Stream, response.ContentType, response.Nombre);
    }
    [HttpGet]
    public async Task<IActionResult> GetZipMuestra(string[] id, string[] id_tipo_archivo)
    {
      var response = await _muestrasService.GetZipMuestra(new DocumentoMuestraRequestViewModel
      {
        id_muestra = id,
        id_tipo_archivo = obtenerTipoArchivo(id_tipo_archivo)
      });

      return File(response.Stream, response.ContentType, response.Nombre);
    }
    [HttpGet]
    public async Task<IActionResult> GetDocumentoInformes(int[] id)
    {
      var response = await _informesService.GetDocumentoInformes(id);
      return File(response.Stream, response.ContentType, response.Nombre);
    }
    [HttpPost]
    public async Task<IActionResult> ObtenerMuestras(FiltrosViewModel filtro, [FromQuery] int page = 1, [FromQuery] int rowPage=20)
    {
      var response = await _muestrasService.Obtener(filtro, page, rowPage);
      AgregarBloque(response.pagina);
      ViewBag.RowPage = rowPage;
      return PartialView("_Muestras", response);
    }

    [HttpPost]
    public async Task<IActionResult> ObtenerEdd()
    {
        var jsonResponse = await _muestrasService.ObtenerEdd();       
        return Content(jsonResponse.ToString(Formatting.None), "application/json");
     }

    [HttpPost]
    public async Task<IActionResult> ObtenerCertificados(FiltrosViewModel filtro, [FromQuery] int page = 1, [FromQuery] int rowPage = 20)
    {
      var response = await _informesService.Obtener(filtro, page, rowPage);
      AgregarBloque(response.pagina);
      ViewBag.RowPage = rowPage;
      return PartialView("_Certificados", response);
    }
    
    [HttpPost]
    public async Task<IActionResult> GetDocumentosMuestra(DocumentoMuestraRequestViewModel model)
    {
      var response = await _muestrasService.GetDocumentosMuestra(model);
      ViewBag.Id = model.id_muestra;
      return PartialView("_Documentos", response);
    }
   
    private string[] obtenerTipoArchivo(string[] id_tipo_archivo)
    {
      if (id_tipo_archivo.Length == 0 || id_tipo_archivo.Contains("0")) return null;
      return id_tipo_archivo;
    }
  }
}

