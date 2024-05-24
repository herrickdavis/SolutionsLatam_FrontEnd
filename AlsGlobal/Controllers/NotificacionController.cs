using AlsGlobal.Extensions;
using AlsGlobal.Models;
using AlsGlobal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using static AlsGlobal.Services.INotificacionService;

namespace AlsGlobal.Controllers
{
    public class NotificacionController : Controller
    {
        private readonly ILogger<DatosPreliminarController> _logger;
        private readonly IPerfilService _perfilService;
        private readonly IAspNetUser _aspNetUser;
        private readonly INotificacionService _notificacionService;

        public NotificacionController(ILogger<DatosPreliminarController> logger,
          INotificacionService notificacionService,
          IAspNetUser aspNetUser, IPerfilService perfilService)
        {
            _logger = logger;
            _aspNetUser = aspNetUser;
            _notificacionService = notificacionService;
            _perfilService = perfilService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetNotificaciones()
        {
            var response = await _notificacionService.GetNotificaciones();

            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }
    }
}
