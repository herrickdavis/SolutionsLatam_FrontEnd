using AlsGlobal.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace AlsGlobal.Extensions
{
  public interface IAspNetUser
  {
    string Name { get; }
    string Email { get; }
    int Rol { get; }
    IEnumerable<MenuViewModel> ObtenerMenu();
    string ObtenerToken();
    bool EstaAutenticado();
    bool MostrarDatosPreliminar();
    HttpContext ObtenerHttpContext();
   
  }
}
