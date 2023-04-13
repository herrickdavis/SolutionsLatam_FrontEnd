using AlsGlobal.Extensions;
using AlsGlobal.Models;
using AlsGlobal.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace AlsGlobal.Controllers
{
  [Authorize]
  public class PerfilController : MainController
  {
    private readonly IEmpresaService _empresaService;
    private readonly IPerfilService _perfilService;
    private readonly IAspNetUser _user;
    public PerfilController(IPerfilService perfilService, IEmpresaService empresaService, IAspNetUser user)
    {
      _perfilService = perfilService;
      _empresaService = empresaService;
      _user = user;
    }
    public IActionResult Index()
    {
      return View();
    }
    public async Task<IActionResult> Empresas()
    {
      var response = await _empresaService.Obtener();

      return View(response);
    }
    public async Task<IActionResult> CambiarPassword(CambiarPasswordRequestViewModel request)
    {
      var response = await _perfilService.CambiarPassword(request);
      return Json(response);
    }
    [HttpGet]
    public async Task<IActionResult> GetColumnas(ColumnasRequestViewModel request){
      var response = await _perfilService.GetColumnas(request);
      return Json(response);
    }
    [HttpPost]
    public async Task<IActionResult> CambiarEmpresa(int id)
    {
      var response = await _empresaService.CambiarEmpresa(id);
      SetCookieValue(id);

      await ActualizarMenu(response.menu);
      return Json(response);
    }
    [HttpPost]
    public async Task<IActionResult> SetColumnasUsuario(ColumnasUsuarioRequestViewModel request)
    {
      var response = await _perfilService.SetColumnasUsuario(request);
      return Json(response);
    }
    private void SetCookieValue(int id)
    {
      HttpContext.Response.Cookies.Append("empresaSeleccionada", id.ToString());
    } 
    private async Task ActualizarMenu(IEnumerable<MenuViewModel> menus)
    {
      var claimsIdentity = (ClaimsIdentity)HttpContext.User.Identity;
      var identity = new ClaimsIdentity(claimsIdentity);
      var myClaim = identity.Claims.FirstOrDefault(x => x.Type == "Menu");

      if (myClaim != null) identity.RemoveClaim(myClaim);
      identity.AddClaim(new Claim("Menu", JsonSerializer.Serialize(menus)));
      var authProperties = new AuthenticationProperties
      {
        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
        IsPersistent = true
      };
      await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
      await HttpContext.SignInAsync(
       CookieAuthenticationDefaults.AuthenticationScheme,
       new ClaimsPrincipal(identity),
       authProperties);
    }
  }
}
