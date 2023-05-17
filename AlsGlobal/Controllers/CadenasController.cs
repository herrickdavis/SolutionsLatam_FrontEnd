using AlsGlobal.Extensions;
using AlsGlobal.Models;
using AlsGlobal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AlsGlobal.Controllers
{
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
            AgregarBloque(response.pagina);            
            ViewBag.RowPage = 20;           
            ViewBag.ColumnasFilter = await _perfilService.GetColumnas(new ColumnasRequestViewModel(1));
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> ObtenerCadenas(int[] id)
        {
            var response = await _cadenasService.GetDocumentosCadena(id);
            return File(response.Stream, response.ContentType, response.Nombre);
        }
    }
}
