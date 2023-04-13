using AlsGlobal.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AlsGlobal.Extensions
{
  public class MainController:Controller
  {
    protected void AgregarBloque<T>(T response) where T:IPagina
    {
      int paginasxBloque = 10;
      int cantidadBloques = response.last_page / paginasxBloque;
      int residuoBloqueActual = response.last_page % paginasxBloque;
      cantidadBloques += residuoBloqueActual == 0 ? 0 : 1;

      int bloqueActual = response.current_page / paginasxBloque;
      residuoBloqueActual = response.current_page % paginasxBloque;
      bloqueActual += residuoBloqueActual == 0 ? 0 : 1;

      if (bloqueActual == cantidadBloques)
      {
        response.PrimerBloque = cantidadBloques == 1;
        response.UltimoBloque = true;
        response.from = (bloqueActual * paginasxBloque) - 9;
        response.to = response.last_page;
      }
      else
      {
        response.PrimerBloque = bloqueActual == 1;
        response.from = (bloqueActual * paginasxBloque) - 9;
        response.to = (bloqueActual * paginasxBloque);
      }
    }
  }
}
