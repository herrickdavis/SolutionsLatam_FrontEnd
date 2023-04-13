using AlsGlobal.Extensions;
using AlsGlobal.Models;
using AlsGlobal.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlsGlobal.Components
{
  public class EmpresaViewComponent:ViewComponent
  {
    private readonly IEmpresaService _empresaService;
    private readonly IAspNetUser _user;
    public EmpresaViewComponent(IEmpresaService empresaService, IAspNetUser user)
    {
      _empresaService = empresaService;
      _user = user;
    }
    public async Task<IViewComponentResult> InvokeAsync()
    {
      if (_user.Rol >= 4)
      {
        ViewBag.Seleccionada = 0;
        return View(new List<EmpresaViewModel>());
      }
      var response = await _empresaService.Obtener();
      ViewBag.Seleccionada = GetCookieValue();
      return View(response);
    }
    private string GetCookieValue()
    {
      return HttpContext.Request.Cookies["empresaSeleccionada"] ?? "-1";
    }
  }
}
