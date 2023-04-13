using AlsGlobal.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlsGlobal.Components
{
  public class PaginaViewComponent:ViewComponent
  {
    public async Task<IViewComponentResult> InvokeAsync(IPagina pagina, string divId, int rowPage)
    {
      ViewBag.Id = divId;
      ViewBag.Select = ObtenerFilasPorPagina(rowPage);
      return View(pagina);
    }
    private IEnumerable<SelectListItem> ObtenerFilasPorPagina(int valueSelected)
    {
      var select = new List<SelectListItem>
      {
        new SelectListItem
        {
          Text = "20",
          Value = "20",
          Selected = 20 == valueSelected
        },
         new SelectListItem
        {
          Text = "50",
          Value = "50",
          Selected = 50 == valueSelected
        },
          new SelectListItem
        {
          Text = "100",
          Value = "100",
          Selected = 100 == valueSelected
        }
      };
      return select;
    }
  }
}
