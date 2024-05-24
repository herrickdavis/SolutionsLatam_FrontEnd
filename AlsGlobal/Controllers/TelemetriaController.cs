using AlsGlobal.Extensions;
using AlsGlobal.Models;
using AlsGlobal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AlsGlobal.Controllers
{
    public class TelemetriaController : MainController
    {
        private readonly ILogger<DatosPreliminarController> _logger;
        private readonly IPerfilService _perfilService;
        private readonly IAspNetUser _aspNetUser;
        private readonly ITelemetriaService _telemetriaService;

        public TelemetriaController(ILogger<DatosPreliminarController> logger,
          ITelemetriaService telemetriaService,
          IAspNetUser aspNetUser, IPerfilService perfilService)
        {
            _logger = logger;
            _aspNetUser = aspNetUser;
            _telemetriaService = telemetriaService;
            _perfilService = perfilService;
        }
        public async Task<IActionResult> CalidadAire()
        {
            return View();
        }

        public async Task<IActionResult> Reglas()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetInfo()
        {
            var response = await _telemetriaService.GetInfo();

            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetData(string[] nombre_estacion, int[] id_parametros)
        {
            var response = await _telemetriaService.GetData(nombre_estacion, id_parametros);

            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetDateWindRose(string[] nombre_estacion, int[] id_parametros)
        {
            var response = await _telemetriaService.GetDateWindRose(nombre_estacion, id_parametros);

            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetAllParametro()
        {
            var response = await _telemetriaService.GetAllParametro();

            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetAllGrupo()
        {
            var response = await _telemetriaService.GetAllGrupo();

            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetAllStation()
        {
            var response = await _telemetriaService.GetAllStation();

            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetAllLimite()
        {
            var response = await _telemetriaService.GetAllLimite();

            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> SetLimite(string nombre_limite, TelemetriaSetLimite[] parametros, int id_limite = 0)
        {
            var response = await _telemetriaService.SetLimite(nombre_limite, parametros, id_limite);

            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        public async Task<IActionResult> SetGrupoParametros(string nombre_grupo_parametro, TelemetriaSetGrupoParametros[] parametros, int id_grupo_parametro = 0)
        {
            var response = await _telemetriaService.SetGrupoParametros(nombre_grupo_parametro, parametros, id_grupo_parametro);

            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }
    }
}
