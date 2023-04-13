using AlsGlobal.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace AlsGlobal.Extensions
{
  public class AspNetUser : IAspNetUser
  {
    private readonly IHttpContextAccessor _httpContext;
    public AspNetUser(IHttpContextAccessor httpContext)
    {
      _httpContext = httpContext;
    }
    public string Name =>  _httpContext.HttpContext.User.GetName();
    public string Email => _httpContext.HttpContext.User.GetEmail();
    public int Rol => _httpContext.HttpContext.User.GetRol();

    public bool EstaAutenticado()
    {
      return _httpContext.HttpContext.User.Identity.IsAuthenticated;
    }
    public bool MostrarDatosPreliminar()
    {
      var data_campo = _httpContext.HttpContext.User.Get_data_campo() ?? "N";
      return data_campo == "S";
    }

    public HttpContext ObtenerHttpContext()
    {
      return _httpContext.HttpContext;
    }

    public IEnumerable<MenuViewModel> ObtenerMenu()
    {
      return _httpContext.HttpContext.User.GetMenu();
    }

    public string ObtenerToken()
    {
      return _httpContext.HttpContext.User.GetToken();
    }
  }
}
