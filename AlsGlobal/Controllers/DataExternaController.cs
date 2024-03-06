using AlsGlobal.Extensions;
using AlsGlobal.Models;
using AlsGlobal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AlsGlobal.Services.IDataExternaService;

namespace AlsGlobal.Controllers
{
    [Authorize]
    public class DataExternaController : MainController
    {
        private readonly IDataExternaService _dataExternaService;
        private readonly ILogger<DatosPreliminarController> _logger;
        private readonly IPerfilService _perfilService;
        private readonly IAspNetUser _aspNetUser;

        public DataExternaController(ILogger<DatosPreliminarController> logger,
          IDataExternaService dataExternaService,
          IAspNetUser aspNetUser, IPerfilService perfilService)
        {
            _logger = logger;
            _aspNetUser = aspNetUser;
            _dataExternaService = dataExternaService;
            _perfilService = perfilService;
        }

        [HttpPost]
        public async Task<IActionResult> ObtenerInfo()
        {
            var response = await _dataExternaService.GetInfo();

            if (response == null)
            {
                return NotFound(); // O manejar de acuerdo a tu lógica de negocio
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ObtenerMuestras(int pagina = 1)
        {
            var response = await _dataExternaService.GetMuestras(pagina);

            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No se proporcionó un archivo.");

            var result = await _dataExternaService.UploadFileDataExterna(file);

            if (result)
                return Ok("Archivo cargado con éxito.");
            else
                return BadRequest("Error al cargar el archivo.");
        }

        [HttpPost]
        public async Task<IActionResult> EnviarMuestras([FromBody]  List<GetDataExternaMuestra> muestras)
        {
            var response = await _dataExternaService.SetMuestras(muestras);

            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetExcelDataExterna()
        {
            var stream = await _dataExternaService.GetExcelDataExterna();

            if (stream == null)
            {
                return NotFound();
            }

            // Asigna un nombre de archivo para el archivo Excel
            string excelFileName = "Datos.xlsx";

            // Devuelve el archivo
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelFileName);
        }

        [HttpPost]
        public async Task<IActionResult> EliminarFila([FromBody] int id)
        {
            var response = await _dataExternaService.EliminarFila(id);
            
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AgregarEstacion()
        {
            var response = await _dataExternaService.agregarEstacion();

            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AgregarProyecto()
        {
            var response = await _dataExternaService.agregarProyecto();

            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }
    }
}
