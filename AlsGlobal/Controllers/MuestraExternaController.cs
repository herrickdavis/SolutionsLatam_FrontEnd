using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlsGlobal.Controllers
{
    [Authorize]
    public class MuestraExternaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
